using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Domain.Entities
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; } = false;
        public List<RoleResponse> Roles { get; set; } = null!;
        public StatusUserResponse Status { get; set; } = null!;

        public UserResponse()
        {
            Roles = new List<RoleResponse>();
        }
    }
}
