using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;
using CampusStay.Brokers.Storages;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using static System.Net.WebRequestMethods;

namespace CampusStay.Brokers.Email
{
    public class EmailBroker : IEmailBroker
    {
        private readonly IConfiguration configuration;
        private readonly IStorageBroker storageBroker;

        public EmailBroker(
            IConfiguration configuration,
            IStorageBroker storageBroker)
        {
            this.configuration = configuration;
            this.storageBroker = storageBroker;
        }

        public async Task SendAccessTokenByEmailAsync(string email, string token)
        {
            if (string.IsNullOrWhiteSpace(this.configuration["Email:EmailAddress"]))
                throw new ArgumentException("Email address is not configured.");

            if (string.IsNullOrWhiteSpace(this.configuration["Email:Password"]))
                throw new ArgumentException("Email password is not configured.");

            using var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(this.configuration["Email:EmailAddress"],
                this.configuration["Email:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.configuration["Email:EmailAddress"]),
                Subject = "Registration Confirmation",
                Body = $"Please use the following link to complete your registration:\n https://55c0-2a05-45c2-301c-ae00-85cd-3e84-3dd0-3457.ngrok-free.app/api/auth/verify?token={token}",
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);
            await smtpClient.SendMailAsync(mailMessage);
        }

        public string GetEmailFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(token);
            return jwtToken.Claims.First(claim => claim.Type == ClaimTypes.Email).Value;
        }

        public async Task CompleteRegistrationForEmailAsync(string email)
        {
            var user = this.storageBroker.SelectAllUsers().FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                user.IsVerified = true;
                await this.storageBroker.UpdateUserAsync(user);
            }
            else
            {
                throw new Exception("User not found.");
            }
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(this.configuration["Email:EmailAddress"],
                this.configuration["Email:Password"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(this.configuration["Email:EmailAddress"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(to);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
