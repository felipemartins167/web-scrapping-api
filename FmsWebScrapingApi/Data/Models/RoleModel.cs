using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Data.Models
{
    public class RoleModel : IRoleModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
