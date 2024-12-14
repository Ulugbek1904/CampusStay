using System;
using System.Collections.Generic;
using System.Linq;

namespace CampusStay.Models.ApartmentModels
{
    public class Apartment
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string ContactInfo { get; set; } = string.Empty;
        public ApartmentPhotos Photos { get; set; }
        public string Description { get; set; } = string.Empty;
        public bool IsAviable { get; set; }
        public DateTime ListedAt { get; set; } = DateTime.UtcNow;
        
        public ICollection<Favorite> Favorites { get; set; }
        public ICollection<Like> Likes { get; set; }
    }

}
