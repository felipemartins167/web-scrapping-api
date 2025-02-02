using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Data.Models
{
    public class StatusUserModel : IStatusUserModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
