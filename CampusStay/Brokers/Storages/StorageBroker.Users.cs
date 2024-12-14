using CampusStay.Models.UserModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace CampusStay.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<User> Users { get; set; }

        public ValueTask<User> InsertUserAsync(User user) =>
            InsertAsync(user);

        public IQueryable<User> SelectAllUsers() =>
            SelectAll<User>();

        public ValueTask<User> SelectUserById(Guid id) =>
            SelectByIdAsync<User>(id);

        public ValueTask<User> UpdateUserAsync(User user) =>
            UpdateAsync(user);

        public ValueTask<User> DeleteUserAsync(User user) =>
            DeleteAsync(user);
    }
}
