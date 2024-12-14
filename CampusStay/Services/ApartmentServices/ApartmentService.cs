using CampusStay.Brokers.Storages;
using CampusStay.Exceptions.ApartmentExceptions;
using CampusStay.Models.ApartmentModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace CampusStay.Services.ApartmentServices
{
    public class ApartmentService : IApartmentService
    {
        private readonly IStorageBroker storageBroker;

        public ApartmentService(IStorageBroker storageBroker)
        {
            this.storageBroker = storageBroker;
        }

        public async Task<Apartment> AddApartmentAsync(Apartment apartment)
        {
            if (apartment == null)
                throw new ApartmentNullException("Apartment not be null");

            await this.storageBroker.InsertApartmentAsync(apartment);

            return apartment;
        }


        public async Task<Apartment> GetApartmentByIdAsync(Guid id)
        {
            var apartment = await 
                this.storageBroker.SelectApartmentById(id);

            return apartment == null ? 
                throw new ApartmentNotFoundException
                ("Apartment not found") : apartment;
        }

        public  async Task<IQueryable<Apartment>> GetMostLikedApartmentsAsync()
        {
            var apartmentWithLikes = await this.storageBroker.SelectAllApartments()
                .Select(apartment => new
                {
                    Apartment = apartment,
                    LikeCount = apartment.Likes.Count()
                })
                .OrderByDescending(a => a.LikeCount)
                .Take(10)
                .ToListAsync();

            return apartmentWithLikes.Select(a => a.Apartment).AsQueryable();
        }

        public async Task<IQueryable<Apartment>> SearchApartmentsAsync(string title, string address)
        {
            var apartments = await this.storageBroker.SelectAllApartments()
                .Where(apartment =>
                    (string.IsNullOrEmpty(title) || apartment.Title.Contains(title)) &&
                    (string.IsNullOrEmpty(title) || apartment.Address.Contains(address)))
                .ToListAsync();

            return apartments.AsQueryable();
        }

        public async Task<Apartment> UpdateApartmentAsync(Apartment apartment)
        {
            var existingApartment = await this.storageBroker.
                SelectApartmentById(apartment.Id);

            if (existingApartment == null)
                throw new ApartmentNullException("Apartment not found");

            return await this.storageBroker.UpdateApartmentAsync(apartment);
        }

        public async Task DeleteApartmentAsync(Guid id)
        {
            var existingApartment = await 
                this.storageBroker.SelectApartmentById(id);

            if (existingApartment == null)
                throw new ApartmentNullException($"Apartment with id {id} not found");

            await this.storageBroker.DeleteApartmentAsync(existingApartment);
        }
    }
}
