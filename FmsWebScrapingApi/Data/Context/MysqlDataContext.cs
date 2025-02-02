using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Infra.Config;
using FmsWebScrapingApi.Infra.Constants;
using MySql.Data.MySqlClient;

namespace FmsWebScrapingApi.Data.Context
{
    public class MysqlDataContext : IDataContext
    {
        private MySqlConnection _conn;

        public MysqlDataContext()
        {
            try
            {
                var configuration = AppSettingsConfig.GetConfiguration();
                string connectionString = configuration[$"ConnectionStrings:{configuration[$"Environment"]}"];
                _conn = new MySqlConnection();
                _conn.ConnectionString = connectionString;
                _conn.Open();
            }
            catch (MySqlException ex)
            {
                throw new ApiException(ErrorMessageConstants.DatabaseConnectionError, ErrorCodeConstants.MysqlConnectionCode, ex);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public MySqlConnection Connection => _conn;

        public void Dispose()
        {
            _conn.Dispose();
            _conn.Close();
        }
    }
}
