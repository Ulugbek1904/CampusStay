using CampusStay.Brokers.Storages;
using CampusStay.Models.ApartmentModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CampusStay.Services.ApartmentServices
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IStorageBroker storageBroker;

        public FavoriteService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async Task<Favorite> AddToFavoritesAsync(Guid userId, Guid apartmentId)
        {
            var favorite = new Favorite()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ApartmentId = apartmentId,
                AddedAt = DateTime.Now,
            };

            return await this.storageBroker.InsertFavoriteAsync(favorite);
        }

        public async Task<IQueryable<Favorite>> GetFavoritesByUserAsync(Guid userId)
        {
            var favorites = this.storageBroker.SelectAllFavorites().
                Where(favorite => favorite.UserId == userId);

            return favorites;
        }

        public async Task RemoveFromFavoritesAsync(Guid favoriteId)
        {
            var favorite = await this.storageBroker.
                SelectFavoriteById(favoriteId);

            await this.storageBroker.DeleteFavoriteAsync(favorite);
        }
    }
}
