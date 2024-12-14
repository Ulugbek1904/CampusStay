using CampusStay.Models.UserModels;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace CampusStay.Brokers.Tokens
{
    public interface ITokenBroker
    {
        public Task SaveResetCodeAsync(string email, string resetCode, DateTime expireTime);
        public (bool IsValid, DateTime ExpireTime) ValidateResetCode(string email, string resetCode);
        RefreshAccessTokens GenerateTokens(string email);
        public Task<bool> VerifyTokenAsync(string token);
        public Task SaveRefreshTokenAsync(string email, string refreshToken);
        public bool ValidateResetPasswordToken(string token);
        public string GenerateResetPasswordToken(User user);
        public Task<bool> ValidateRefreshTokenAsync(string refreshToken, string email);
    }
}