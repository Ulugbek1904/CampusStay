using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CampusStay.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Favorite> InsertFavoriteAsync(Favorite favorite);
        IQueryable<Favorite> SelectAllFavorites();
        ValueTask<Favorite> SelectFavoriteById(Guid id);
        ValueTask<Favorite> UpdateFavoriteAsync(Favorite favorite);
        ValueTask<Favorite> DeleteFavoriteAsync(Favorite favorite);
    }
}
