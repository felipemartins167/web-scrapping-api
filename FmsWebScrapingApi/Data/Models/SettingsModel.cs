using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Data.Models
{
    public class SettingsModel : ISettingsModel
    {
        public int Id { get; set; }
        public int ExpirationTokenHours { get; set; }
    }
}
