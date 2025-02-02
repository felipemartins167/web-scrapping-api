namespace FmsWebScrapingApi.Domain.Entities
{
    public class ResetPasswordDto
    {
        public required string Token { get; set; } = string.Empty;
        public required string Password { get; set; } = string.Empty;
        public required string Email { get; set; }
    }
}
