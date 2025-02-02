using FmsWebScrapingApi.Data.Models;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Domain.Interfaces;
using MySql.Data.MySqlClient;

namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository
    {
        public Task<int> CreateUser(UserRequest userRequest, string hashEmail, MySqlTransaction transaction);
        public Task<List<UserResponse>> GetPaginateUsers(int pageNumber, int pageSize, string search);
        public Task<IUserModel?> GetUserByLoginRole(UserLoginDto userLoginDto, int role);
        public Task<IUserModel?> GetUserById(int id);
        public Task<IUserModel?> GetUserByEmail(string email);
        public Task<IUserModel?> GetUserByRefreshToken(string refreshToken);
        public Task<bool> CreateRefreshTokenUser(UserTokenDto refreshToken, int userId, MySqlTransaction transaction);
        public Task<bool> UpdateRefreshTokenUser(UserTokenDto refreshToken, string oldRefreshToken, int userId, MySqlTransaction transaction);
        public Task<bool> UpdatePasswordUser(string password, int userId, MySqlTransaction transaction);
        public Task<bool> CreateHashTokenPassword(HashTokenPassword hashTokenPassword);
        public Task<HashTokenPassword> GetHashTokenPasswordByToken(string token, string email);
        public Task<bool> DeleteHashTokenPasswordByToken(string token, MySqlTransaction transaction);
        public Task<bool> AddRoleUser(int roleId, int userId, MySqlTransaction transaction);
        public Task<bool> ValidateUserEmailByToken(string token, MySqlTransaction transaction);
        public Task<int> GetTotalPaginateUsers(string search);
    }
}
