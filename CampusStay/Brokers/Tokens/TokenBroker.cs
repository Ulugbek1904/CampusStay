using CampusStay.Brokers.Storages;
using CampusStay.Exceptions.UserExceptions;
using CampusStay.Models.UserModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CampusStay.Brokers.Email;
using Microsoft.EntityFrameworkCore;
using CampusStay.Services.UserServices;
using System.Collections.Generic;

namespace CampusStay.Brokers.Tokens
{
    public class TokenBroker : ITokenBroker
    {
        private readonly IConfiguration configuration;
        private readonly IStorageBroker storageBroker;
        private readonly IUserService userService;
        private readonly IEmailBroker emailBroker;

        public TokenBroker(
            IConfiguration configuration, 
            IStorageBroker storageBroker,
            IUserService userService,
            IEmailBroker emailBroker)
        {
            this.configuration = configuration;
            this.storageBroker = storageBroker;
            this.userService = userService;
            this.emailBroker = emailBroker;
        }


        private readonly Dictionary<string, (string ResetCode, DateTime ExpireTime)> resetCodes = new();

        public async Task SaveResetCodeAsync(string email, string resetCode, DateTime expireTime)
        {
            resetCodes[email] = (resetCode, expireTime.ToUniversalTime()); 
            await Task.CompletedTask;
        }

        public (bool IsValid, DateTime ExpireTime) ValidateResetCode(string email, string resetCode)
        {
            if (resetCodes.TryGetValue(email, out var value))
            {
                if (value.ResetCode == resetCode)
                {
                    if (DateTime.UtcNow > value.ExpireTime)
                    {
                        return (false, value.ExpireTime); 
                    }
                    return (true, value.ExpireTime); 
                }
            }
            return (false, DateTime.MinValue); 
        }



        public RefreshAccessTokens GenerateTokens(string email)
        {
            var accessToken = GenerateAccessToken(email);
            var refreshToken = GenerateRefreshToken();

            return new RefreshAccessTokens
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }

        public async Task<bool> VerifyTokenAsync(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(this.configuration["JWT:Key"]);

            try
            {
                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = this.configuration["JWT:Issuer"],
                    ValidAudience = this.configuration["JWT:Audience"],
                    ClockSkew = TimeSpan.FromMinutes(5)
                }, out SecurityToken validatedToken);

                var email = principal.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
                await this.emailBroker.CompleteRegistrationForEmailAsync(email);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Token validation failed: {ex.Message}");
                return false;
            }
        }

        public async Task SaveRefreshTokenAsync(string email, string refreshToken)
        {
            var user = this.storageBroker.SelectAllUsers().
                FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);
                await this.storageBroker.UpdateUserAsync(user);
            }
            else
            {
                throw new UserNotFoundException("User not found.");
            }

        }

        public async Task<bool> ValidateRefreshTokenAsync(string refreshToken, string email)
        {
            var user = await this.userService.GetUserByEmailAsync(email);

            if (user == null || user.RefreshToken != refreshToken)
            {
                return false; 
            }

            if (user.RefreshTokenExpiryTime <= DateTime.UtcNow)
            {
                return false; 
            }

            return true;
        }

        public string GenerateResetPasswordToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(this.configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] { new Claim("email", user.Email) }),
                Expires = DateTime.UtcNow.AddMinutes(15),
                Issuer = this.configuration["JWT:Issuer"],
                Audience = this.configuration["JWT:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public bool ValidateResetPasswordToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var key = Encoding.UTF8.GetBytes(this.configuration["JWT:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GetStoredRefreshToken(string email)
        {
            var user = this.storageBroker.SelectAllUsers().FirstOrDefault(x => x.Email == email);

            return user.RefreshToken;
        }

        private string GenerateAccessToken(string email)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.configuration["JWT:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, this.configuration["JWT:Subject"]),
                new Claim(ClaimTypes.Email, email) 
            };

            var token = new JwtSecurityToken(
                issuer: this.configuration["JWT:Issuer"],
                audience: this.configuration["JWT:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

    }
    public class RefreshAccessTokens
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}