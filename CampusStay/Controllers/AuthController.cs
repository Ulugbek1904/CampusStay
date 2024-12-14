using CampusStay.Brokers.Email;
using CampusStay.Brokers.Tokens;
using CampusStay.DTO.Requests;
using CampusStay.DTO.Users;
using CampusStay.Models.UserModels;
using CampusStay.Services.UserServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using RESTFulSense.Controllers;
using Serilog;
using System;
using System.Net.Mail;
using System.Threading.Tasks;

namespace CampusStay.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : RESTFulController
    {
        private readonly IUserService userService;
        private readonly IConfiguration configuration;
        private readonly ITokenBroker tokenBroker;
        private readonly IEmailBroker emailBroker;

        public AuthController(
            IUserService userService,
            IConfiguration configuration,
            ITokenBroker tokenBroker,
            IEmailBroker emailBroker)
        {
            this.userService = userService;
            this.configuration = configuration;
            this.tokenBroker = tokenBroker;
            this.emailBroker = emailBroker;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto userDto)
        {
            if (string.IsNullOrWhiteSpace(userDto.Email) ||
                string.IsNullOrWhiteSpace(userDto.Password) ||
                string.IsNullOrWhiteSpace(userDto.ConfirmPassword))
            {
                return BadRequest("Email and passwords cannot be empty.");
            }

            if (userDto.Password != userDto.ConfirmPassword)
                return BadRequest("Passwords do not match.");

            if (!IsValidEmail(userDto.Email))
                return BadRequest("Invalid email format.");

            var existingUser = await this.userService.GetUserByEmailAsync(userDto.Email);

            if (existingUser != null)
            {
                if (existingUser.IsVerified)
                {
                    return BadRequest("This email is already registered and verified." +
                        "You can log in");
                }
                else
                {
                    var tokens = this.tokenBroker.GenerateTokens(existingUser.Email);
                    await this.emailBroker.SendAccessTokenByEmailAsync(existingUser.Email, tokens.AccessToken);

                    return Ok("This email is already registered but not verified." +
                        " A new verification link has been sent to your email.");
                }
            }
            try
            {
                var user = new User
                {
                    Name = userDto.Name,
                    Surname = userDto.Surname,
                    PhoneNumber = userDto.PhoneNumber,
                    Email = userDto.Email,
                    Password = userDto.Password,
                    ConfirmPassword = userDto.ConfirmPassword,
                    IsVerified = false,
                    CreatedAt = DateTime.UtcNow
                };

                await this.userService.AddUserAsync(user);

                var tokens = this.tokenBroker.GenerateTokens(user.Email);
                await this.emailBroker.SendAccessTokenByEmailAsync(user.Email, tokens.AccessToken);

                return Created("User registered successfully. Please verify your email.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("verify")]
        public async Task<IActionResult> VerifyEmail(string token)
        {
            var isTokenValid = await this.tokenBroker.VerifyTokenAsync(token);

            if (!isTokenValid)
                return BadRequest("Invalid or expired token");

            var email = this.emailBroker.GetEmailFromToken(token);
            if (email == null)
                return BadRequest("Invalid token or user not found.");

            var user = await this.userService.GetUserByEmailAsync(email);
            if (user == null)
                return BadRequest("User not found.");

            if (user.IsVerified)
            {
                var isVerified = await this.userService.MarkUserAsVerifiedAsync(email);

                if (isVerified)
                    return Ok("Email verified successfully. You can now log in.");
                else
                    return BadRequest("Unable to verify email.");
            }

            return BadRequest("Email already verified.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUserRequest request)
        {
            var ipAddress = HttpContext.Connection.RemoteIpAddress?.ToString();
            try
            {
                var user = await this.userService.GetUserByEmailAsync(request.Email);

                if (user == null)
                {
                    Log.Warning("Logging failed : User not found. Email:" +
                        " {Email}, IP: {IP}, Time: {Time}", request.Email, ipAddress, DateTime.UtcNow);

                    return BadRequest("Firstly, you need to sign up or check email validity");
                }

                if (!this.userService.VerifyPassword(user, request.Password))
                {
                    Log.Warning("Logging failed: Incorrect password. Email:" +
                        " {Email}, IP: {IP}, Time: {Time}", request.Email, ipAddress, DateTime.UtcNow);

                    return Unauthorized("Password is incorrect");
                }

                if (!user.IsVerified)
                {
                    Log.Warning("Logging failed: Email not verified. Email:" +
                        " {Email}, IP: {IP}, Time: {Time}", request.Email, ipAddress, DateTime.UtcNow);

                    return BadRequest("Please verify your email first.");
                }

                var tokens = this.tokenBroker.GenerateTokens(request.Email);
                await this.tokenBroker.SaveRefreshTokenAsync(user.Email, tokens.RefreshToken);

                Log.Information("Logging succesfully. Email: {email}," +
                    " IP: {ip}, Time: {time}", request.Email, ipAddress, DateTime.UtcNow);

                return Created(tokens);
            }
            catch (Exception ex)
            {
                Log.Error("Login xatosi: {Error}, Email: {Email}, IP: {IP}," +
                    " Time: {Time}", ex.Message, request.Email, ipAddress, DateTime.UtcNow);

                return StatusCode(500, "Server xatosi");
            }

            
        }

        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequest request)
        {
            var user = await this.userService.GetUserByEmailAsync(request.Email);

            if (user == null)
                return Unauthorized("User not found");

            var isValidRefreshToken = await this.tokenBroker.
                ValidateRefreshTokenAsync(request.RefreshToken, request.Email);

            if (!isValidRefreshToken)
                return Unauthorized("Invalid or expired refresh token");

            var newTokens = this.tokenBroker.GenerateTokens(request.Email);

            await this.tokenBroker.SaveRefreshTokenAsync(request.Email, newTokens.RefreshToken);

            return Ok(newTokens.AccessToken); 
        }

        [HttpPost("password-reset-request")]
        public async Task<IActionResult> PasswordResetRequest(string email)
        {
            var user = await this.userService.GetUserByEmailAsync(email);
            if (user == null)
                return BadRequest("User with this email does not exist.");

            var resetCode = new Random().Next(100000, 999999).ToString(); 
            var expireTime = DateTime.UtcNow.AddMinutes(1); 

            await this.tokenBroker.SaveResetCodeAsync(email, resetCode, expireTime);

            await this.emailBroker.SendEmailAsync(email, "Password Reset Request", $"Your reset code is {resetCode}. It will expire in 15 minutes.");

            return Ok("Reset code sent to your email.");
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto model)
        {
            var user = await this.userService.GetUserByEmailAsync(model.Email);
            if (user == null)
                return BadRequest("User not found.");

            var (isValidCode, expireTime) = this.tokenBroker
                .ValidateResetCode(model.Email, model.ResetCode);

            if (isValidCode)
                return BadRequest("Invalid or expired reset code.");

            if (model.NewPassword != model.ConfirmPassword)
                return BadRequest("New password and confirm password do not match.");

            user.Password = model.NewPassword;
            user.ConfirmPassword = model.ConfirmPassword;
            await this.userService.UpdateUserAsync(user);

            return Ok("Password reset successfully.");
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
