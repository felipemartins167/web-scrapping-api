namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface ISettingsModel
    {
        public int Id { get; set; }
        public int ExpirationTokenHours { get; set; }
    }
}
