using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Models;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Domain.Interfaces;

namespace FmsWebScrapingApi.Services.Interfaces
{
    public interface IAuthService
    {
        public Task<IUserModel> CreateUser(UserRequest userRequest);
        public Task<IUserModel> LoginUserByRoleId(UserLoginDto userLoginDto, int roleId);
        public Task<bool> CreateRefreshToken(UserTokenDto refreshToken, int id);
        public Task<bool> UpdateRefreshToken(UserTokenDto newRefreshToken, string oldRefreshToken, int userId);
        public Task<bool> ValidateUserEmailByToken(string token);
        public Task SendEmailResetPasswordUser(string email);
        public Task ResetPasswordUser(ResetPasswordDto resetPasswordDto);
        public Task<IUserModel> GetUserByRefreshToken(RefreshTokenDto refreshTokenDto);
        public Task<IUserModel> GetUserById(int id);
    }
}
