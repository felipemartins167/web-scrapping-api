using MySql.Data.MySqlClient;

namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IDataContext : IDisposable
    {
        MySqlConnection Connection { get; }
    }
}
