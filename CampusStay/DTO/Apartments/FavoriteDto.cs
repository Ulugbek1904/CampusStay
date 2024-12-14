using System;

namespace CampusStay.DTO.Apartments
{
    public class FavoriteDto
    {
        public Guid Id { get; set; }
        public Guid ApartmentId { get; set; }
        public ApartmentDto Apartment { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
