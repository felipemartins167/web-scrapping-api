namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IAddressModel
    {
        public int Id { get; set; }
        public string? Number { get; set; }
        public string Street { get; set; }
        public string Neighborhood { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string? ZipCode { get; set; }
        public string? AdditionalAddressDetails { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public string City { get; set; }
        public DateTime? CreateAt { get; set; }
        public DateTime? UpdateAt { get; set; }
    }
}
