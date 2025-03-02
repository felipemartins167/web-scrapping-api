namespace FmsWebScrapingApi.Domain.Entities
{
    public class ProductResponse
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public required decimal Reviews { get; set; }
        public int ReviewsTotal { get; set; }
        public string Url { get; set; } = string.Empty;
        public required string MarketPlace { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public decimal PriceFrom { get; set; }
        public decimal PriceTo { get; set; }
        public string PriceLocal { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
