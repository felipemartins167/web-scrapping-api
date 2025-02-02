using FmsWebScrapingApi.Data.Context;

namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IBaseRepository
    {
        public MysqlDataContext DbContext { get; }
    }
}
