using FmsWebScrapingApi.Domain.Interfaces;

namespace FmsWebScrapingApi.Domain.Entities
{
    public class UserLoginDto
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
