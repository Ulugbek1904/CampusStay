using CampusStay.Models.CartModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CampusStay.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<CartItems> CartItems { get; set; }
        public ValueTask<CartItems> InsertCartItemsAsync(CartItems cartItems) =>
            InsertAsync(cartItems);

        public IQueryable<CartItems> SelectAllCartItems() =>
            SelectAll<CartItems>();

        public ValueTask<CartItems> SelectCartItemsById(Guid id) =>
            SelectByIdAsync<CartItems>(id);

        public ValueTask<CartItems> UpdateCartItemsAsync(CartItems cartItems) =>
            UpdateAsync(cartItems);

        public ValueTask<CartItems> DeleteCartItemsAsync(CartItems cartItems) =>
            DeleteAsync(cartItems);
    }
}
