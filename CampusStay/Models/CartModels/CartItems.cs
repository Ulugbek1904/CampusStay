using CampusStay.Models.ApartmentModels;
using System;
using System.Text.Json.Serialization;

namespace CampusStay.Models.CartModels
{
    public class CartItems
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ApartmentId { get; set; }
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public Apartment Apartment { get; set; }
    }
}
