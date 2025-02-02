using FmsWebScrapingApi.Domain.Interfaces;

namespace FmsWebScrapingApi.Domain.Entities
{
    public class UserTokenDto
    {
        public string Token { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
    }
}
