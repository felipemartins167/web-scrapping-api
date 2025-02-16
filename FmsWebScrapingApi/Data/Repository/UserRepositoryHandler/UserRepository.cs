using System.Data;
using System.Data.Common;
using System.Xml.Linq;
using FmsWebScrapingApi.Data.Context;
using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Models;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Domain.Interfaces;
using FmsWebScrapingApi.Infra.Config;
using FmsWebScrapingApi.Infra.Constants;
using FmsWebScrapingApi.Infra.Helpers.Encrypt;
using MySql.Data.MySqlClient;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FmsWebScrapingApi.Data.Repository.UserRepositoryHandler
{
    public class UserRepository : IUserRepository
    {
        public MysqlDataContext DbContext { get; }
        private MySqlConnection _conn;

        public UserRepository()
        {
            DbContext = new MysqlDataContext();
            _conn = DbContext.Connection;
        }

        public async Task<bool> CreateRefreshTokenUser(UserTokenDto refreshToken, int userId, MySqlTransaction transaction)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetUserByEmailPassword = UserSqlStatments.CreateRefreshTokenUser();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("refreshToken", refreshToken.RefreshToken);
                cmd.Parameters.AddWithValue("token", refreshToken.Token);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("expiration", DateTime.UtcNow.AddHours(Convert.ToInt32(_config["Jwt:ExpirationMinutes"])));
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<IUserModel?> GetUserByLoginRole(UserLoginDto userLoginDto, int role)
        {
            try
            {
                UserModel? userModel = null;
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetUserByEmailPassword = UserSqlStatments.GetUserByLoginRole();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("email", userLoginDto.Email);
                cmd.Parameters.AddWithValue("password", EncryptHelper.EncryptString(userLoginDto.Password));
                cmd.Parameters.AddWithValue("role", role);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        userModel = new UserModel()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader["name"].ToString(),
                            Status = new StatusUserModel()
                            {
                                Id = Convert.ToInt32(reader["status_id"]),
                                Name = reader.IsDBNull(reader.GetOrdinal("name_status")) ? "" : reader["name_status"].ToString()
                            }
                        };
                    }
                }

                return userModel;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<IUserModel?> GetUserById(int id)
        {
            try
            {
                UserModel? userModel = null;
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetUserByEmailPassword = UserSqlStatments.GetUserById();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("id", id);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        userModel = new UserModel()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader["name"].ToString(),
                            Status = new StatusUserModel()
                            {
                                Id = Convert.ToInt32(reader["status_id"]),
                                Name = reader.IsDBNull(reader.GetOrdinal("name_status")) ? "" : reader["name_status"].ToString()
                            }
                        };
                    }
                }

                return userModel;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<bool> UpdateRefreshTokenUser(UserTokenDto refreshToken, string oldRefreshToken, int userId, MySqlTransaction transaction)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                cmd.Transaction = transaction;
                string sqlGetUserByEmailPassword = UserSqlStatments.UpdateRefreshTokenUser();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("refreshToken", refreshToken.RefreshToken);
                cmd.Parameters.AddWithValue("oldRefreshToken", oldRefreshToken);
                cmd.Parameters.AddWithValue("token", refreshToken.Token);
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("expiration", DateTime.UtcNow.AddHours(Convert.ToInt32(_config["Jwt:ExpirationMinutes"])));
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<IUserModel?> GetUserByEmail(string email)
        {
            try
            {
                UserModel? userModel = null;
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetUserByEmailPassword = UserSqlStatments.GetUserByEmail();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("email", email);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        userModel = new UserModel()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader["name"].ToString(),
                            Status = new StatusUserModel()
                            {
                                Id = Convert.ToInt32(reader["status_id"]),
                                Name = reader.IsDBNull(reader.GetOrdinal("name_status")) ? "" : reader["name_status"].ToString()
                            }
                        };
                    }
                }

                return userModel;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<bool> CreateHashTokenPassword(HashTokenPassword hashTokenPassword)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlCreateHashTokenPassword = UserSqlStatments.CreateHashTokenPassword();
                cmd.CommandText = sqlCreateHashTokenPassword;
                cmd.Parameters.AddWithValue("token", hashTokenPassword.Token);
                cmd.Parameters.AddWithValue("userId", hashTokenPassword.UserId);
                cmd.Parameters.AddWithValue("expiration", hashTokenPassword.Expiration);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<HashTokenPassword> GetHashTokenPasswordByToken(string token, string email)
        {
            try
            {
                HashTokenPassword? hashTokenPassword = null;
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlHashTokenPassword = UserSqlStatments.GetHashTokenPasswordByToken();
                cmd.CommandText = sqlHashTokenPassword;
                cmd.Parameters.AddWithValue("token", token);
                cmd.Parameters.AddWithValue("email", email);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        hashTokenPassword = new HashTokenPassword()
                        {
                            Token = reader.IsDBNull(reader.GetOrdinal("token")) ? "" : reader["token"].ToString(),
                            Expiration = reader.IsDBNull(reader.GetOrdinal("expiration")) ? DateTime.UtcNow : Convert.ToDateTime(reader["expiration"]),
                            UserId = Convert.ToInt32(reader["user_id"])
                        };
                    }
                }

                return hashTokenPassword;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<bool> UpdatePasswordUser(string password, int userId, MySqlTransaction transaction)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                cmd.Transaction = transaction;
                string sqlGetUserByEmailPassword = UserSqlStatments.UpdatePasswordUserById();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("password", EncryptHelper.EncryptString(password));
                cmd.Parameters.AddWithValue("userId", userId);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<bool> DeleteHashTokenPasswordByToken(string token, MySqlTransaction transaction)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                cmd.Transaction = transaction;
                string sqlDeleteHashTokenByToken = UserSqlStatments.DeleteHashTokenByToken();
                cmd.CommandText = sqlDeleteHashTokenByToken;
                cmd.Parameters.AddWithValue("token", token);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<IUserModel?> GetUserByRefreshToken(string refreshToken)
        {
            try
            {
                UserModel? userModel = null;
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetUserByEmailPassword = UserSqlStatments.GetUserByRefreshToken();
                cmd.CommandText = sqlGetUserByEmailPassword;
                cmd.Parameters.AddWithValue("refreshToken", refreshToken);
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        userModel = new UserModel()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader["name"].ToString(),
                            Status = new StatusUserModel()
                            {
                                Id = Convert.ToInt32(reader["status_id"]),
                                Name = reader.IsDBNull(reader.GetOrdinal("name_status")) ? "" : reader["name_status"].ToString()
                            }
                        };
                    }
                }

                return userModel;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<int> CreateUser(UserRequest userRequest, string hashEmail, MySqlTransaction transaction)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                cmd.Transaction = transaction;
                string sqlCreateUser = UserSqlStatments.CreateUser();
                cmd.CommandText = sqlCreateUser;
                cmd.Parameters.AddWithValue("name", userRequest.Name);
                cmd.Parameters.AddWithValue("email", userRequest.Email);
                cmd.Parameters.AddWithValue("password", EncryptHelper.EncryptString(userRequest.Password));
                cmd.Parameters.AddWithValue("hashEmail", hashEmail);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    cmd.CommandText = "SELECT LAST_INSERT_ID();";
                    int insertedId = Convert.ToInt32(await cmd.ExecuteScalarAsync());
                    return insertedId;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, $"Data: {ex.Data}. Message: {ex.Message}.");
            }
        }

        public async Task<bool> AddRoleUser(int roleId, int userId, MySqlTransaction transaction)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                cmd.Transaction = transaction;
                string sqlCreateUserRole = UserSqlStatments.CreateUserRole();
                cmd.CommandText = sqlCreateUserRole;
                cmd.Parameters.AddWithValue("userId", userId);
                cmd.Parameters.AddWithValue("roleId", roleId);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<bool> ValidateUserEmailByToken(string token, MySqlTransaction transaction)
        {
            try
            {
                IConfiguration _config = AppSettingsConfig.GetConfiguration();
                MySqlCommand cmd = _conn.CreateCommand();
                cmd.Transaction = transaction;
                string sqlValidateEmail = UserSqlStatments.ValidateUserEmailByToken();
                cmd.CommandText = sqlValidateEmail;
                cmd.Parameters.AddWithValue("hash_email_confirm", token);
                cmd.Parameters.AddWithValue("email_confirmed", 1);
                int rowsAffected = await cmd.ExecuteNonQueryAsync();
                if (rowsAffected > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<List<UserResponse>> GetPaginateUsers(int pageNumber, int pageSize, string search)
        {
            try
            {
                var usersRoles = new List<UserRoleDto>();
                MySqlCommand cmd = _conn.CreateCommand();
                string sqlGetUsersRoles = UserSqlStatments.GetUsersRoles();
                cmd.CommandText = sqlGetUsersRoles;
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        usersRoles.Add(new UserRoleDto()
                        {
                            UserId = Convert.ToInt32(reader["user_id"]),
                            RoleId = Convert.ToInt32(reader["role_id"]),
                            RoleName = reader.IsDBNull(reader.GetOrdinal("name_role")) ? "" : reader["name_role"].ToString()
                        });
                    }
                }

                var users = new List<UserResponse>();
                string sqlGetUsers = UserSqlStatments.GetUsers();
                
                cmd.Parameters.AddWithValue("limit", pageSize);
                cmd.Parameters.AddWithValue("offset", (pageNumber - 1) * pageSize);

                if (search != null)
                {
                    cmd.Parameters.AddWithValue("search", $"%{search}%");
                    sqlGetUsers += UserSqlStatments.SearchUser();
                }

                sqlGetUsers += UserSqlStatments.PaginationUser();

                cmd.CommandText = sqlGetUsers;
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var userId = Convert.ToInt32(reader["id"]);
                        var roleResponses = new List<RoleResponse>();
                        var rolesUser = usersRoles.Where(x => x.UserId == userId);
                        foreach (var roleResponse in rolesUser)
                        {
                            roleResponses.Add(new RoleResponse()
                            {
                                Id = roleResponse.RoleId,
                                Name = roleResponse.RoleName
                            });
                        }
                        users.Add(new UserResponse()
                        {
                            Id = Convert.ToInt32(reader["id"]),
                            Name = reader.IsDBNull(reader.GetOrdinal("name")) ? "" : reader["name"].ToString(),
                            Email = reader.IsDBNull(reader.GetOrdinal("email")) ? "" : reader["email"].ToString(),
                            Status = new StatusUserResponse()
                            {
                                Id = Convert.ToInt32(reader["status_id"]),
                                Name = reader.IsDBNull(reader.GetOrdinal("name_status")) ? "" : reader["name_status"].ToString()
                            },
                            Roles = roleResponses
                        });
                    }
                }

                return users;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<int> GetTotalPaginateUsers(string search)
        {
            try
            {
                MySqlCommand cmd = _conn.CreateCommand();

                string sqlGetUsers = UserSqlStatments.GetTotalUsers();

                if (search != null)
                {
                    cmd.Parameters.AddWithValue("search", $"%{search}%");
                    sqlGetUsers += UserSqlStatments.SearchUser();
                }

                cmd.CommandText = sqlGetUsers;
                int totalRows = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                return totalRows;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }
    }
}
