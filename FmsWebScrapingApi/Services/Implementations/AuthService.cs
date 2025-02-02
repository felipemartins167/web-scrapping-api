using System.ComponentModel.DataAnnotations;
using System.Security.Policy;
using EstacaoDoOlhoApi.Infra.Helpers.Email;
using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Models;
using FmsWebScrapingApi.Data.Repository;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Domain.Interfaces;
using FmsWebScrapingApi.Domain.Rules;
using FmsWebScrapingApi.Infra.Config;
using FmsWebScrapingApi.Infra.Constants;
using FmsWebScrapingApi.Infra.Helpers.Encrypt;
using FmsWebScrapingApi.Services.Interfaces;
using MySql.Data.MySqlClient;

namespace FmsWebScrapingApi.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private IUserRepository _userRepository;
        private IEmailRepository _emailRepository;
        private ISettingsRepository _settingsRepository;

        public AuthService(IUserRepository userRepository, IEmailRepository emailRepository, ISettingsRepository settingsRepository)
        {
            _userRepository = userRepository;
            _emailRepository = emailRepository;
            _settingsRepository = settingsRepository;
        }

        public async Task<bool> CreateRefreshToken(UserTokenDto refreshToken, int id)
        {
            try
            {
                var transaction = await _userRepository.DbContext.Connection.BeginTransactionAsync();
                bool isRefreshTokenInsert = await _userRepository.CreateRefreshTokenUser(refreshToken, id, transaction);
                transaction.Commit();
                if (isRefreshTokenInsert)
                {
                    return true;
                }
                else
                {
                    throw new ApiException(ErrorMessageConstants.InsertTokenUser, ErrorCodeConstants.InsertTokenUser, null);
                }
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

        public async Task<IUserModel> GetUserByRefreshToken(RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var userModel = await _userRepository.GetUserByRefreshToken(refreshTokenDto.RefreshToken);
                if (userModel == null)
                {
                    throw new AuthException(ErrorMessageConstants.UserNotFoundError, ErrorCodeConstants.UserNotFoundCode, null);
                }
                else
                {
                    return userModel;
                }
            }
            catch (AuthException)
            {
                throw;
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

        public async Task<IUserModel> GetUserById(int id)
        {
            try
            {
                var userModel = await _userRepository.GetUserById(id);
                if (userModel == null)
                {
                    throw new AuthException(ErrorMessageConstants.UserNotFoundError, ErrorCodeConstants.UserNotFoundCode, null);
                }
                else
                {
                    return userModel;
                }
            }
            catch (AuthException)
            {
                throw;
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

        public async Task<IUserModel> LoginUserByRoleId(UserLoginDto userLoginDto, int roleId)
        {
            try
            {

                var userModel = await _userRepository.GetUserByLoginRole(userLoginDto, roleId);
                if (userModel == null)
                {
                    throw new AuthException(ErrorMessageConstants.UserNotFoundError, ErrorCodeConstants.UserNotFoundCode, null);
                }
                else
                {
                    return userModel;
                }
            }
            catch (AuthException)
            {
                throw;
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

        public async Task ResetPasswordUser(ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var hashTokenPassword = await _userRepository.GetHashTokenPasswordByToken(resetPasswordDto.Token, resetPasswordDto.Email);

                if (hashTokenPassword == null)
                {
                    throw new ApiException(ErrorMessageConstants.SecurityInvalidToken, ErrorCodeConstants.SecurityInvalidToken, null);
                }

                var transaction = await _userRepository.DbContext.Connection.BeginTransactionAsync();

                bool isInsertPassword = await _userRepository.UpdatePasswordUser(resetPasswordDto.Password, hashTokenPassword.UserId, transaction);

                if (isInsertPassword)
                {
                    await _userRepository.DeleteHashTokenPasswordByToken(resetPasswordDto.Token, transaction);
                    await transaction.CommitAsync();
                    return;
                }
                else
                {
                    await transaction.RollbackAsync();
                    throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, null);
                }
            }
            catch (AuthException)
            {
                throw;
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

        public async Task SendEmailResetPasswordUser(string email)
        {
            try
            {
                ValidationResult validateEmail = new AuthRules(_userRepository).ValidateEmail(email);
                if (validateEmail != null)
                {
                    throw new ApiException(ErrorMessageConstants.InvalidEmail, ErrorCodeConstants.InvalidEmail, validateEmail);
                }
                var userModel = _userRepository.GetUserByEmail(email);
                if (userModel == null)
                {
                    return;
                }
                var emailTemplate = await _emailRepository.GetEmailTemplateByTemplateIdentifier(EmailConstants.EMAIL_TEMPLATE_EMAIL_RESET_PASSWORD);
                if (emailTemplate == null)
                {
                    throw new ApiException(ErrorMessageConstants.InvalidEmailTemplate, ErrorCodeConstants.InvalidEmailTemplate, null);
                }
                var settings = await _settingsRepository.GetSettings();

                if (settings == null)
                {
                    throw new ApiException(ErrorMessageConstants.SettingsNotFound, ErrorCodeConstants.SettingsNotFound, null);
                }

                var hashTokenPassword = new HashTokenPassword()
                {
                    Expiration = DateTime.UtcNow.AddHours(settings.ExpirationTokenHours),
                    Token = new TokenManager().GenerateCustomTokenUrl(16),
                    UserId = userModel.Result.Id
                };

                emailTemplate.Message = emailTemplate.Message.Replace("{{NOME}}", userModel.Result.Name);

                var configuration = AppSettingsConfig.GetConfiguration();
                string urlSite = configuration[$"UrlSite:{configuration[$"Environment"]}"];

                string url = $"{urlSite}/recovery/password?token={hashTokenPassword.Token}";

                emailTemplate.Message = emailTemplate.Message.Replace("{{LINK}}", url);

                bool isHashTokenCreated = await _userRepository.CreateHashTokenPassword(hashTokenPassword);

                if (!isHashTokenCreated)
                {
                    throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, null);
                }

                await EmailHelper.SendMail([email], emailTemplate.Message, "felipemartins167@hotmail.com");
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

        public async Task<bool> UpdateRefreshToken(UserTokenDto newRefreshToken, string oldRefreshToken, int userId)
        {
            try
            {
                MySqlTransaction transaction = await _userRepository.DbContext.Connection.BeginTransactionAsync();
                bool isRefreshTokenUpdated = await _userRepository.UpdateRefreshTokenUser(newRefreshToken, oldRefreshToken, userId, transaction);
                if (!isRefreshTokenUpdated)
                {
                    throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, null);
                }
                transaction.Commit();
                return true;
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

        public async Task<bool> ValidateUserEmailByToken(string token)
        {
            try
            {
                MySqlTransaction transaction = await _userRepository.DbContext.Connection.BeginTransactionAsync();
                bool isTokenValidated = await _userRepository.ValidateUserEmailByToken(token, transaction);
                if (!isTokenValidated)
                {
                    throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, null);
                }
                transaction.Commit();
                return true;
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

        public async Task<IUserModel> CreateUser(UserRequest userRequest)
        {
            MySqlTransaction transaction = await _userRepository.DbContext.Connection.BeginTransactionAsync();
            try
            {
                List<ValidationResult> validateCreateUser = await new AuthRules(_userRepository).ValidateCreateUser(userRequest);
                if (validateCreateUser != null && validateCreateUser.Count > 0)
                {
                    throw new ApiException(ErrorMessageConstants.CreateUser, ErrorCodeConstants.CreateUser, validateCreateUser);
                }
                string hashEmailUser = new TokenManager().GenerateCustomToken(10);
                int idUser = await _userRepository.CreateUser(userRequest, hashEmailUser, transaction);
                bool isRoleAdded = await _userRepository.AddRoleUser(userRequest.RoleId, idUser, transaction);
                if (idUser == 0)
                {
                    transaction.Rollback();
                    throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, null);
                }
                transaction.Commit();

                var emailTemplate = await _emailRepository.GetEmailTemplateByTemplateIdentifier(EmailConstants.EMAIL_TEMPLATE_EMAIL_VALIDATION);
                if (emailTemplate == null)
                {
                    throw new ApiException(ErrorMessageConstants.InvalidEmailTemplate, ErrorCodeConstants.InvalidEmailTemplate, null);
                }

                var configuration = AppSettingsConfig.GetConfiguration();
                string urlSite = configuration[$"UrlSite:{configuration[$"Environment"]}"];

                string url = $"{urlSite}/validation/email?token={hashEmailUser}";
                emailTemplate.Message = emailTemplate.Message.Replace("{{LINK}}", url);
                emailTemplate.Message = emailTemplate.Message.Replace("{{NOME}}", userRequest.Name);

                await EmailHelper.SendMail([userRequest.Email], emailTemplate.Message, "felipemartins167@hotmail.com");

                var userModel = await _userRepository.GetUserById(idUser);
                return userModel!;
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
