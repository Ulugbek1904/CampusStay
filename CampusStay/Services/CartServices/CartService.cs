using CampusStay.Brokers.Storages;
using CampusStay.Models.CartModels;
using Microsoft.AspNetCore.Cors.Infrastructure;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CampusStay.Services.CartServices
{
    public class CartService : ICartService
    {
        private readonly IStorageBroker storageBroker;

        public CartService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async Task<CartItems> AddToCartAsync(Guid userId, Guid apartmentId)
        {
            CartItems cartItems = new CartItems()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                ApartmentId = apartmentId,
                AddedAt = DateTime.UtcNow,
            };

            return await this.storageBroker.InsertCartItemsAsync(cartItems);
        }

        public async Task<IQueryable<CartItems>> GetCartItemsByUserAsync(Guid userId) =>
            this.storageBroker.SelectAllCartItems().
                Where(cart => cart.UserId == userId);

        public async Task RemoveFromCartAsync(Guid cartItemId)
        {
            CartItems cartItem = await this.storageBroker.SelectCartItemsById(cartItemId);

            await this.storageBroker.DeleteAsync(cartItem);
        }
    }
}
