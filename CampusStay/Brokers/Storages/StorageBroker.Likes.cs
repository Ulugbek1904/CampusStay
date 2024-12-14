using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CampusStay.Brokers.Storages
{
    public partial class StorageBroker
    {
        private DbSet<Like> Likes { get; set; }
        public ValueTask<Like> InsertLikeAsync(Like like) =>
            InsertAsync(like);

        public IQueryable<Like> SelectAllLikes(Like like) =>
            SelectAll<Like>();

        public ValueTask<Like> SelectLikeById(Guid id) =>
            SelectByIdAsync<Like>(id);

        public ValueTask<Like> UpdateLikeAsync(Like like) =>
            UpdateAsync(like);

        public ValueTask<Like> DeleteLikeAsync(Like like) =>
            DeleteAsync(like);
    }
}
