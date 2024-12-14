using CampusStay.Models.CartModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CampusStay.Services.CartServices
{
    public interface ICartService
    {
        Task<CartItems> AddToCartAsync(Guid userId, Guid apartmentId);
        Task<IQueryable<CartItems>> GetCartItemsByUserAsync(Guid userId);
        Task RemoveFromCartAsync(Guid cartItemId);
    }
}