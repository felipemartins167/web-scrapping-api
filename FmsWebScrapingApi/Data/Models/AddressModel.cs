using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Data.Models
{
    public class AddressModel : IAddressModel
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string Street { get; set; } = string.Empty;
        public string Neighborhood { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string? ZipCode { get; set; }
        public string? AdditionalAddressDetails { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string City { get; set; } = string.Empty;
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; } = DateTime.UtcNow;
    }
}
