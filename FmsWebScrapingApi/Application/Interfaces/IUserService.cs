using FmsWebScrapingApi.Domain.Entities;

namespace FmsWebScrapingApi.Services.Interfaces
{
    public interface IUserService
    {
        public Task<List<UserResponse>> GetPaginateUsers(int pageNumber, int pageSize, string search);
        public Task<int> GetTotalPaginateUsers(string search, int pageSize);
        public Task<UserResponse> GetUserById(int userId);
    }
}
