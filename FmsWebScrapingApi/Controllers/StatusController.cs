using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Infra.Helpers.Encrypt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FmsWebScrapingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StatusController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet("v1/check")]
        public async Task<IActionResult> CheckStatusApi()
        {
            IActionResult result = Ok();
            try
            {
                result = Ok(new ApiResponse<dynamic>(new { Version = "0.0.1" }, false, null, null, null));
            }
            catch (AuthException ex)
            {
                result = StatusCode(401, new ApiResponse<AuthException>(ex, true, ex.ErrorMessage, null, null));
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
