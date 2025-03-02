namespace FmsWebScrapingApi.Data.Repository.ProductRepositoryHandler
{
    public class ProductSqlStatments
    {
        public static string GetProducts()
        {
            return $"select p.product_id as id, p.product_name, p.reviews, p.reviews_qtd, p.created_date, p.product_url, p.created_date, p.marketplace, p.modified_date, p.product_image, p.product_name, p.product_price_from, p.product_price_local, p.product_price_to from products p ";
        }

        public static string GetTotalProducts()
        {
            return $"select count(*) from products ";
        }

        public static string SearchProduct()
        {
            return $" where product_name like @search or marketplace like @search ";
        }

        public static string PaginationProduct()
        {
            return $" order by p.reviews_qtd desc LIMIT @limit OFFSET @offset";
        }

        public static string GetProductByIdMarketPlace()
        {
            return $"select p.product_id as id, p.product_name, p.reviews, p.reviews_qtd, p.created_date, p.product_url, p.created_date, p.marketplace, p.modified_date, p.product_image, p.product_name, p.product_price_from, p.product_price_local, p.product_price_to from products p where p.product_id = @id and marketplace = @marketPlace ";
        }
    }
}
