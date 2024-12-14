using CampusStay.Models.ApartmentModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CampusStay.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        ValueTask<Apartment> InsertApartmentAsync(Apartment apartment);
        IQueryable<Apartment> SelectAllApartments();
        ValueTask<Apartment> SelectApartmentById(Guid id);
        ValueTask<Apartment> UpdateApartmentAsync(Apartment apartment);
        ValueTask<Apartment> DeleteApartmentAsync(Apartment apartment);
    }
}
