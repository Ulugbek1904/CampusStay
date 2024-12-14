using CampusStay.Models.ApartmentModels;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace CampusStay.Brokers.Storages
{
    public partial interface IStorageBroker
    {
        public ValueTask<ApartmentPhotos> InsertApartmentPhotoAsync(ApartmentPhotos apartmentPhotos);
        public IQueryable<ApartmentPhotos> SelectAllApartmentPhotos();
        public ValueTask<ApartmentPhotos> SelectApartmentPhotoById(Guid id);
        public ValueTask<ApartmentPhotos> UpdateApartmentPhotoAsync(ApartmentPhotos apartmentPhotos);
        public ValueTask<ApartmentPhotos> DeleteApartmentPhotoAsync(ApartmentPhotos apartmentPhotos);
    }
}
