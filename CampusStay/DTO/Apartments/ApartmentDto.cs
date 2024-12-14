using System.Collections.Generic;
using System;

namespace CampusStay.DTO.Apartments
{
    public class ApartmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public string ContactInfo { get; set; }
        public List<ApartmentPhotoDto> Photos { get; set; } = new List<ApartmentPhotoDto>();
        public string Description { get; set; }
        public int Likes { get; set; }
        public bool IsFavorite { get; set; }
        public bool IsAvailable { get; set; }
        public DateTime ListedAt { get; set; }
    }
}
