using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CampusStay.Services.ApartmentServices
{
    public interface IApartmentService
    {
        Task<Apartment> AddApartmentAsync(Apartment apartment);
        Task<Apartment> GetApartmentByIdAsync(Guid id);
        Task<IQueryable<Apartment>> SearchApartmentsAsync(string title, string address);
        Task<IQueryable<Apartment>> GetMostLikedApartmentsAsync();
        Task<Apartment> UpdateApartmentAsync(Apartment apartment);
        Task DeleteApartmentAsync(Guid id);
    }
}