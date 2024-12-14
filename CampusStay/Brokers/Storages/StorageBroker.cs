using CampusStay.Models.ApartmentModels;
using CampusStay.Models.ChatModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using STX.EFxceptions.SqlServer;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CampusStay.Brokers.Storages
{
    public partial class StorageBroker : EFxceptionsContext, IStorageBroker
    {
        private IConfiguration configuration;
        public StorageBroker(IConfiguration configuration)
        {
            this.configuration = configuration;
            this.Database.Migrate();
        }

        public async ValueTask<T> InsertAsync<T>(T entity) where T : class
        {
            await this.Set<T>().AddAsync(entity);
            await this.SaveChangesAsync();

            return entity;
        }

        public IQueryable<T> SelectAll<T>() where T : class
        {
            return this.Set<T>();
        }

        public async ValueTask<T> SelectByIdAsync<T>(Guid id) where T : class
        {
            return await this.Set<T>().FindAsync(id);
        }

        public async ValueTask<T> UpdateAsync<T>(T entity) where T : class
        {
            this.Set<T>().Update(entity);
            await this.SaveChangesAsync();

            return entity;
        }
        public async ValueTask<T> DeleteAsync<T>(T entity) where T : class
        {
            this.Set<T>().Remove(entity);
            await this.SaveChangesAsync();

            return entity;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = this.configuration.GetConnectionString("DefaultConnectionString");
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Favorite>()
            .HasOne(f => f.User)
            .WithMany(u => u.Favorites)
            .HasForeignKey(f => f.UserId);

            modelBuilder.Entity<Favorite>()
                .HasOne(f => f.Apartment)
                .WithMany(a => a.Favorites)
                .HasForeignKey(f => f.ApartmentId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.User)
                .WithMany(u => u.Likes)
                .HasForeignKey(l => l.UserId);

            modelBuilder.Entity<Like>()
                .HasOne(l => l.Apartment)
                .WithMany(a => a.Likes)
                .HasForeignKey(l => l.ApartmentId);

            modelBuilder.Entity<Message>()
                .HasKey(message => message.Id);

            modelBuilder.Entity<Chat>()
                .HasKey(chat => chat.Id);
        }
    }
}
