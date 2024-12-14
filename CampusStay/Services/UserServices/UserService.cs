using CampusStay.Brokers.Email;
using CampusStay.Brokers.Storages;
using CampusStay.Exceptions.UserExceptions;
using CampusStay.Models.UserModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CampusStay.Services.UserServices
{
    public class UserService : IUserService
    {
        private readonly IStorageBroker storageBroker;
        private readonly IEmailBroker emailBroker;

        public UserService(
            IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async Task<User> AddUserAsync(User user)
        {
            if (user == null) 
                throw new UserNullException("user not be null");

            await this.storageBroker.InsertUserAsync(user);

            return user;
        }

        public IQueryable<User> GetAllUsersAsync()
        {
            return this.storageBroker.SelectAllUsers();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            var user = await this.
                storageBroker.SelectUserById(id);

            return user == null ? 
                throw new UserNotFoundException("User not found")
                    : user;
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            var existingUser = await this.storageBroker.SelectUserById(user.Id);

            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            await storageBroker.UpdateUserAsync(user);

            return user;
        }

        public async Task DeleteUserAsync(Guid id)
        {
            var existingUser = await this.storageBroker.SelectUserById(id);

            if (existingUser == null)
            {
                throw new UserNotFoundException("User not found");
            }

            await this.storageBroker.DeleteUserAsync(existingUser);
        }

        public async Task<bool> UserExistsAsync(string email) =>
            await Task.FromResult(GetAllUsersAsync().Any(u => u.Email == email));

        public bool VerifyPassword(User user, string password) =>
            user.Password == password;

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = GetAllUsersAsync().
                FirstOrDefault(u => u.Email == email);

            return await Task.FromResult(user);
        }

        public async Task<bool> MarkUserAsVerifiedAsync(string email)
        {
            var users = GetAllUsersAsync(); 
            var user = users.FirstOrDefault(u => u.Email == email);

            if (user != null)
            {
                user.IsVerified = true;
                await UpdateUserAsync(user);

                return true;
            }

            return false;
        }

        public async Task ResetPasswordAsync(User user, string newPassword)
        {
            user.Password = newPassword;
            user.ConfirmPassword = newPassword;

            await UpdateUserAsync(user);
        }

        public Task SaveRefreshTokenAsync(string email, string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            var existingUser = await GetUserByEmailAsync(email);
            return existingUser != null;
        }

    }
}
