using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CampusStay.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Favorite> Favorites { get; set; }
        public ValueTask<Favorite> InsertFavoriteAsync(Favorite favorite) =>
            InsertAsync(favorite);

        public IQueryable<Favorite> SelectAllFavorites() =>
            SelectAll<Favorite>();

        public ValueTask<Favorite> SelectFavoriteById(Guid id) =>
            SelectByIdAsync<Favorite>(id);
        public ValueTask<Favorite> UpdateFavoriteAsync(Favorite favorite) =>
            UpdateAsync(favorite);

        public ValueTask<Favorite> DeleteFavoriteAsync(Favorite favorite) =>
            DeleteAsync(favorite);
    }
}
