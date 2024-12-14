namespace CampusStay.DTO.Apartments
{
    public class SearchApartmentDto
    {
        public string Title { get; set; }
        public string Address { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsAvailable { get; set; }
        public string ForWhom { get; set; }
    }
}
