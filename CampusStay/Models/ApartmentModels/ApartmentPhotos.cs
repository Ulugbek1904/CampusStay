using System;
using System.Text.Json.Serialization;

namespace CampusStay.Models.ApartmentModels
{
    public class ApartmentPhotos
    {
        public Guid Id { get; set; }
        public string PhotoUrl { get; set; } = string.Empty;
        public bool isMain { get; set; } = false;
        public Guid ApartmentId { get; set; }
    }
}
