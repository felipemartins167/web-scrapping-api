namespace FmsWebScrapingApi.Infra.Config
{
    public static class AppSettingsConfig
    {
        public static IConfiguration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);
            var configuration = builder.Build();
            return configuration;
        }
    }
}
