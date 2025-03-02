using FmsWebScrapingApi.Data.Context;
using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Repository.UserRepositoryHandler;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Infra.Constants;
using MySql.Data.MySqlClient;
using System.Data.Common;

namespace FmsWebScrapingApi.Data.Repository.ProductRepositoryHandler
{
    public class ProductRepository : IProductRepository
    {
        public MysqlDataContext DbContext { get; }
        private MySqlConnection _conn;

        public ProductRepository()
        {
            DbContext = new MysqlDataContext();
            _conn = DbContext.Connection;
        }

        public async Task<List<ProductResponse>> GetPaginateUsers(int pageNumber, int pageSize, string search)
        {
            try
            {
                var productList = new List<ProductResponse>();
                string sqlGetUsers = ProductSqlStatments.GetProducts();
                MySqlCommand cmd = _conn.CreateCommand();

                cmd.CommandText = sqlGetUsers;

                cmd.Parameters.AddWithValue("limit", pageSize);
                cmd.Parameters.AddWithValue("offset", (pageNumber - 1) * pageSize);

                if (search != null)
                {
                    cmd.Parameters.AddWithValue("search", $"%{search}%");
                    sqlGetUsers += ProductSqlStatments.SearchProduct();
                }

                sqlGetUsers += ProductSqlStatments.PaginationProduct();

                cmd.CommandText = sqlGetUsers;
                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        productList.Add(new ProductResponse()
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("id")) ? "" : reader["id"].ToString(),
                            MarketPlace = reader.IsDBNull(reader.GetOrdinal("marketplace")) ? "" : reader["marketplace"].ToString(),
                            Name = reader.IsDBNull(reader.GetOrdinal("product_name")) ? "" : reader["product_name"].ToString(),
                            Reviews = Convert.ToInt32(reader["reviews"]),
                            ReviewsTotal = Convert.ToInt32(reader["reviews_qtd"]),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("product_image")) ? "" : reader["product_image"].ToString(),
                            Url = reader.IsDBNull(reader.GetOrdinal("product_url")) ? "" : reader["product_url"].ToString(),
                            PriceFrom = Convert.ToDecimal(reader["product_price_from"]),
                            PriceLocal = reader.IsDBNull(reader.GetOrdinal("product_price_local")) ? "" : reader["product_price_local"].ToString(),
                            PriceTo = Convert.ToDecimal(reader["product_price_to"]),
                            CreatedAt = DateTime.Parse(reader.IsDBNull(reader.GetOrdinal("created_date")) ? "" : reader["created_date"].ToString()),
                            UpdatedAt = DateTime.Parse(reader.IsDBNull(reader.GetOrdinal("modified_date")) ? "" : reader["modified_date"].ToString()),
                        });
                    }
                }
                return productList; 
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<int> GetTotalPaginateProducts(string search)
        {
            try
            {
                MySqlCommand cmd = _conn.CreateCommand();

                string sqlGetUsers = ProductSqlStatments.GetTotalProducts();

                if (search != null)
                {
                    cmd.Parameters.AddWithValue("search", $"%{search}%");
                    sqlGetUsers += ProductSqlStatments.SearchProduct();
                }

                cmd.CommandText = sqlGetUsers;
                int totalRows = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                return totalRows;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }

        public async Task<List<ProductResponse>> GetByIdMarketPlace(string id, string marketPlace)
        {
            try
            {
                var product = new List<ProductResponse>();
                string sqlGetProductByNameMarketPlace = ProductSqlStatments.GetProductByIdMarketPlace();
                MySqlCommand cmd = _conn.CreateCommand();

                cmd.Parameters.AddWithValue("id", id);
                cmd.Parameters.AddWithValue("marketPlace", marketPlace);

                cmd.CommandText = sqlGetProductByNameMarketPlace;

                using (DbDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        product.Add(new ProductResponse()
                        {
                            Id = reader.IsDBNull(reader.GetOrdinal("id")) ? "" : reader["id"].ToString(),
                            MarketPlace = reader.IsDBNull(reader.GetOrdinal("marketplace")) ? "" : reader["marketplace"].ToString(),
                            Name = reader.IsDBNull(reader.GetOrdinal("product_name")) ? "" : reader["product_name"].ToString(),
                            Reviews = Convert.ToInt32(reader["reviews"]),
                            ReviewsTotal = Convert.ToInt32(reader["reviews_qtd"]),
                            ImageUrl = reader.IsDBNull(reader.GetOrdinal("product_image")) ? "" : reader["product_image"].ToString(),
                            Url = reader.IsDBNull(reader.GetOrdinal("product_url")) ? "" : reader["product_url"].ToString(),
                            PriceFrom = Convert.ToDecimal(reader["product_price_from"]),
                            PriceLocal = reader.IsDBNull(reader.GetOrdinal("product_price_local")) ? "" : reader["product_price_local"].ToString(),
                            PriceTo = Convert.ToDecimal(reader["product_price_to"]),
                            CreatedAt = DateTime.Parse(reader.IsDBNull(reader.GetOrdinal("created_date")) ? "" : reader["created_date"].ToString()),
                            UpdatedAt = DateTime.Parse(reader.IsDBNull(reader.GetOrdinal("modified_date")) ? "" : reader["modified_date"].ToString()),
                        });
                    }
                }
                return product;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }
        }
    }
}
