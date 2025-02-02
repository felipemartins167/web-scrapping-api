using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Models;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Services.Interfaces;

namespace FmsWebScrapingApi.Services.Implementations
{
    public class UserService : IUserService
    {
        public IUserRepository _userRepository { get; set; }
        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<UserResponse>> GetPaginateUsers(int pageNumber, int pageSize, string search)
        {
            try
            {
                var users = await _userRepository.GetPaginateUsers(pageNumber, pageSize, search);
                return users;
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<int> GetTotalPaginateUsers(string search, int pageSize)
        {
            try
            {
                var totalRows = await _userRepository.GetTotalPaginateUsers(search);
                int total = (int)Math.Ceiling((double)totalRows / pageSize);
                return total;
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
