namespace FmsWebScrapingApi.Domain.Entities
{
    public class UserRequest
    {
        public required string Name { get; set; } = string.Empty;
        public required string Email { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required int RoleId { get; set; }
    }
}
