using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CampusStay.Brokers.Storages
{
    public  partial interface IStorageBroker
    {
        ValueTask<Like> InsertLikeAsync(Like like);
        IQueryable<Like> SelectAllLikes(Like like);
        ValueTask<Like> SelectLikeById(Guid id);
        ValueTask<Like> UpdateLikeAsync(Like like);
        ValueTask<Like> DeleteLikeAsync(Like like);
    }
}
