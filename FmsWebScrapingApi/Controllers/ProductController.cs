using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FmsWebScrapingApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        /// <summary>
        /// Endpoint para carregar a listagem de produtos de forma paginada
        /// </summary>
        /// <param name="pageNumber">Página atual</param>
        /// <param name="pageSize">Número de registros por página</param>
        /// <param name="search">String de busca</param>
        /// <returns>Listagem paginada de produtos</returns>
        [HttpGet("v1/{pageNumber}/{pageSize}/{search}")]
        public async Task<IActionResult> GetAllProducts(int pageNumber, int pageSize, string search)
        {
            IActionResult result = Ok();
            try
            {
                search = search.Replace("{", "").Replace("}", "");
                var products = await _productService.GetPaginateProducts(pageNumber, pageSize, search);
                int total = await _productService.GetTotalPaginateProducts(search, pageSize);
                result = Ok(new ApiResponse<List<ProductResponse>>(products, false, null, pageNumber, total));
            }
            catch (ApiException ex)
            {
                result = StatusCode(400, new ApiResponse<ApiException>(ex, true, ex.ErrorMessage, null, null));
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ApiResponse<Exception>(ex, true, ex.Message, null, null));
            }
            return result;
        }

        [HttpGet("getByIdMarketPlace/v1/{id}/{marketPlace}")]
        public async Task<IActionResult> GetByNameMarketPlace(string id, string marketPlace)
        {
            IActionResult result = Ok();
            try
            {
                id = id.Replace("{", "").Replace("}", "");
                marketPlace = marketPlace.Replace("{", "").Replace("}", "");
                var product = await _productService.GetByIdMarketPlace(id, marketPlace);
                result = Ok(new ApiResponse<List<ProductResponse>>(product, false, null, null, null));
            }
            catch (ApiException ex)
            {
                result = StatusCode(400, new ApiResponse<ApiException>(ex, true, ex.ErrorMessage, null, null));
            }
            catch (Exception ex)
            {
                result = StatusCode(500, new ApiResponse<Exception>(ex, true, ex.Message, null, null));
            }
            return result;
        }
    }
}
