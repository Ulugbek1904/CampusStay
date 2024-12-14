using CampusStay.DTO.Apartments;
using System;

namespace CampusStay.DTO.CartItem
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid ApartmentId { get; set; }
        public ApartmentDto Apartment { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
