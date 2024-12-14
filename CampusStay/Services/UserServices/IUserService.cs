using CampusStay.Models.UserModels;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace CampusStay.Services.UserServices
{
    public interface IUserService
    {
        Task<bool> UserExistsAsync(string email);
        bool VerifyPassword(User user, string password);
        Task<User> GetUserByEmailAsync(string email);
        public Task<bool> MarkUserAsVerifiedAsync(string email);
        Task SaveRefreshTokenAsync(string email, string refreshToken);
        Task<User> AddUserAsync(User user);
        Task<User> GetUserByIdAsync(Guid id);
        IQueryable<User> GetAllUsersAsync();
        Task<User> UpdateUserAsync(User user);
        Task DeleteUserAsync(Guid id);
        public Task<bool> IsEmailTakenAsync(string email);
        public Task ResetPasswordAsync(User user, string newPassword);
    }
}