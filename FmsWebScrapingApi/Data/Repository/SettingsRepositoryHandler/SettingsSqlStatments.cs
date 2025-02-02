namespace FmsWebScrapingApi.Data.Repository.SettingsRepositoryHandler
{
    public static class SettingsSqlStatments
    {
        public static string GetSettings()
        {
            return $"select id, expiration_token_hours from settings";
        }
    }
}
