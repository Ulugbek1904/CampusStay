using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;

namespace CampusStay.Brokers.Storages
{
    public partial class StorageBroker
    {
        DbSet<ApartmentPhotos> ApartmentPhotos { get; set; }
        public ValueTask<ApartmentPhotos> InsertApartmentPhotoAsync(ApartmentPhotos apartmentPhotos) =>
            InsertAsync(apartmentPhotos);

        public IQueryable<ApartmentPhotos> SelectAllApartmentPhotos() =>
            SelectAll<ApartmentPhotos>();

        public ValueTask<ApartmentPhotos> SelectApartmentPhotoById(Guid id) =>
            SelectByIdAsync<ApartmentPhotos>(id);

        public ValueTask<ApartmentPhotos> UpdateApartmentPhotoAsync(ApartmentPhotos apartmentPhotos) =>
            UpdateAsync(apartmentPhotos);

        public ValueTask<ApartmentPhotos> DeleteApartmentPhotoAsync(ApartmentPhotos apartmentPhotos) =>
            DeleteAsync(apartmentPhotos);
    }
}
