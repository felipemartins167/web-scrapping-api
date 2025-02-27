using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FmsWebScrapingApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService)
        {
            this._userService = userService;
        }

        [HttpGet("v1/{pageNumber}/{pageSize}/{search}")]
        public async Task<IActionResult> GetAllUsers(int pageNumber, int pageSize, string search)
        {
            IActionResult result = Ok();
            try
            {
                search = search.Replace("{", "").Replace("}", "");
                var users = await _userService.GetPaginateUsers(pageNumber, pageSize, search);
                int total = await _userService.GetTotalPaginateUsers(search, pageSize);
                result = Ok(new ApiResponse<List<UserResponse>>(users, false, null, pageNumber, total));
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
