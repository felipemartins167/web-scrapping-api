using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Data.Models
{
    public class HashTokenPassword : IHashTokenPassword
    {
        public required string Token { get; set; }
        public required DateTime Expiration { get; set; }
        public required int UserId { get; set; }
    }
}
