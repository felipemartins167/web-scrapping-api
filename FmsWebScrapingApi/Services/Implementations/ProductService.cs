using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Repository.UserRepositoryHandler;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Services.Interfaces;

namespace FmsWebScrapingApi.Services.Implementations
{
    public class ProductService : IProductService
    {
        IProductRepository _productRepository { get; set; }

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<ProductResponse>> GetPaginateProducts(int pageNumber, int pageSize, string search)
        {
            try
            {
                var users = await _productRepository.GetPaginateUsers(pageNumber, pageSize, search);
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

        public async Task<int> GetTotalPaginateProducts(string search, int pageSize)
        {
            try
            {
                var totalRows = await _productRepository.GetTotalPaginateProducts(search);
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

        public async Task<List<ProductResponse>> GetByNameMarketPlace(string name, string marketPlace)
        {
            try
            {
                var product = await _productRepository.GetByNameMarketPlace(name, marketPlace);
                return product;
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
