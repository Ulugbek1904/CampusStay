using CampusStay.Models.UserModels;
using System;
using System.Text.Json.Serialization;

namespace CampusStay.Models.ApartmentModels
{
    public class Favorite
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public Guid ApartmentId { get; set; } 
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public User User { get; set; } 
        public Apartment Apartment { get; set; } 
    }

}
