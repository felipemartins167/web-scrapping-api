namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool EmailConfirmed { get; set; }
        public IRoleModel Role { get; set; }
        public IStatusUserModel Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
