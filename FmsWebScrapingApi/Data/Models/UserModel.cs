using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Data.Models
{
    public class UserModel : IUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool EmailConfirmed { get; set; }
        public IRoleModel Role { get; set; }
        public IStatusUserModel Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public UserModel()
        {
            Role = new RoleModel();
            Status = new StatusUserModel();
        }
    }
}
