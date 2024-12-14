using System;

namespace CampusStay.DTO.Apartments
{
    public class LikeDto
    {
        public Guid Id { get; set; }
        public Guid ApartmentId { get; set; }
        public Guid UserId { get; set; }
        public DateTime LikedAt { get; set; }
    }
}
