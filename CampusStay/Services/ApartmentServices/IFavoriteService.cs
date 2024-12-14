using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CampusStay.Services.ApartmentServices
{
    public interface IFavoriteService
    {
        Task<Favorite> AddToFavoritesAsync(Guid userId, Guid apartmentId);
        Task<IQueryable<Favorite>> GetFavoritesByUserAsync(Guid userId);
        Task RemoveFromFavoritesAsync(Guid favoriteId);
    }
}