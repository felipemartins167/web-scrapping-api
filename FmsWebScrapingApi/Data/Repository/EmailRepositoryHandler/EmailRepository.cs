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

namespace FmsWebScrapingApi.Data.Repository.EmailRepositoryHandler
{
    public class EmailRepository : IEmailRepository
    {
        public MysqlDataContext DbContext { get; }
        private MySqlConnection _conn;

        public EmailRepository()
        {
            DbContext = new MysqlDataContext();
            _conn = DbContext.Connection;
        }

        public async Task<IEmailTemplate>? GetEmailTemplateByTemplateIdentifier(string templateIdentifier)
        {
            try
            {
                EmailTemplate? emailTemplate = null;
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetUserByEmailPassword = EmailSqlStatments.GetEmailTemplateByTemplateIdentifier();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("templateId", templateIdentifier);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        emailTemplate = new EmailTemplate()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Message = reader.IsDBNull(reader.GetOrdinal("message")) ? "" : reader["message"].ToString(),
                            TemplateIdentifier = reader.IsDBNull(reader.GetOrdinal("template_identifier")) ? "" : reader["template_identifier"].ToString(),
                            CreatedAt = reader.IsDBNull(reader.GetOrdinal("create_at")) ? DateTime.UtcNow : Convert.ToDateTime(reader["create_at"]),
                            UpdatedAt = reader.IsDBNull(reader.GetOrdinal("update_at")) ? DateTime.UtcNow : Convert.ToDateTime(reader["update_at"]),
                        };
                    }
                }

                return emailTemplate;
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
