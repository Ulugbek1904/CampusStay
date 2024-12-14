using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CampusStay.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<Apartment> Apartment { get; set; }
        public ValueTask<Apartment> InsertApartmentAsync(Apartment apartment) =>
            InsertAsync(apartment);

        public IQueryable<Apartment> SelectAllApartments() =>
            SelectAll<Apartment>();

        public ValueTask<Apartment> SelectApartmentById(Guid id) =>
            SelectByIdAsync<Apartment>(id);

        public ValueTask<Apartment> UpdateApartmentAsync(Apartment apartment) =>
            UpdateAsync(apartment);

        public ValueTask<Apartment> DeleteApartmentAsync(Apartment apartment) =>
            DeleteAsync(apartment);
    }
}
