using FmsWebScrapingApi.Domain.Entities;

namespace FmsWebScrapingApi.Services.Interfaces
{
    public interface IProductService
    {
        public Task<List<ProductResponse>> GetPaginateProducts(int pageNumber, int pageSize, string search);
        public Task<List<ProductResponse>> GetByNameMarketPlace(string name, string marketPlace);
        public Task<int> GetTotalPaginateProducts(string search, int pageSize);
    }
}
