using System;

namespace CampusStay.DTO.Apartments
{
    public class ApartmentPhotoDto
    {
        public Guid Id { get; set; }
        public string PhotoUrl { get; set; }
        public bool IsMain { get; set; }
    }
}
