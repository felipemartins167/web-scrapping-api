namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface ISettingsRepository : IBaseRepository
    {
        public Task<ISettingsModel?> GetSettings();
    }
}
