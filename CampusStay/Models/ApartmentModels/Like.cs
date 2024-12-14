using CampusStay.Models.UserModels;
using System;
using System.Text.Json.Serialization;

namespace CampusStay.Models.ApartmentModels
{
    public class Like
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; } 
        public Guid ApartmentId { get; set; } 
        public DateTime LikedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore]
        public User User { get; set; }
        [JsonIgnore]
        public Apartment Apartment { get; set; } 
    }

}
