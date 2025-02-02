using System.Data;
using System.Data.Common;
using FmsWebScrapingApi.Data.Context;
using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Models;
using FmsWebScrapingApi.Data.Repository.UserRepositoryHandler;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Infra.Constants;
using FmsWebScrapingApi.Infra.Helpers.Encrypt;
using MySql.Data.MySqlClient;

namespace FmsWebScrapingApi.Data.Repository.SettingsRepositoryHandler
{
    public class SettingsRepository : ISettingsRepository
    {
        public MysqlDataContext DbContext { get; }
        private MySqlConnection _conn;

        public SettingsRepository()
        {
            DbContext = new MysqlDataContext();
            _conn = DbContext.Connection;
        }

        public async Task<ISettingsModel?> GetSettings()
        {
            try
            {
                SettingsModel? settingsModel = null;
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetSettings = SettingsSqlStatments.GetSettings();
                cmd.CommandText = sqlGetSettings;
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        settingsModel = new SettingsModel()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            ExpirationTokenHours = Convert.ToInt32(reader.GetOrdinal("expiration_token_hours"))
                        };
                    }
                }

                return settingsModel;
            }
            catch (ApiException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }
    }
}
