using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using CampusStay.Models.CartModels;

namespace CampusStay.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<CartItems> InsertCartItemsAsync(CartItems cartItems);
        IQueryable<CartItems> SelectAllCartItems();
        ValueTask<CartItems> SelectCartItemsById(Guid id);
        ValueTask<CartItems> UpdateCartItemsAsync(CartItems cartItems);
        ValueTask<CartItems> DeleteCartItemsAsync(CartItems cartItems);
    }
}
