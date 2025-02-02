namespace FmsWebScrapingApi.Domain.Entities
{
    public class UserRoleDto
    {
        public required int UserId { get; set; }
        public required int RoleId { get; set; }
        public required string RoleName { get; set; }
    }
}
