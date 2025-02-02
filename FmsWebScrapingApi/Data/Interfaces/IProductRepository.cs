﻿using FmsWebScrapingApi.Domain.Entities;

namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IProductRepository : IBaseRepository
    {
        public Task<List<ProductResponse>> GetByNameMarketPlace(string name, string marketPlace);
        public Task<List<ProductResponse>> GetPaginateUsers(int pageNumber, int pageSize, string search);
        public Task<int> GetTotalPaginateProducts(string search);
    }
}
