using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Data.Models;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Domain.Interfaces;
using FmsWebScrapingApi.Infra.Helpers.Encrypt;
using FmsWebScrapingApi.Services.Implementations;
using FmsWebScrapingApi.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FmsWebScrapingApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("v1/user")]
        public async Task<IActionResult> CreateUser([FromBody] UserRequest userRequest)
        {
            IActionResult result = Ok();
            try
            {
                var userModel = await _authService.CreateUser(userRequest);
                var userTokenModel = new TokenManager().GetTokenRefreshToken(userModel);
                await _authService.CreateRefreshToken(userTokenModel, userModel.Id);
                result = Ok(new ApiResponse<UserTokenDto>(userTokenModel, false, null, null, null));
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

        [AllowAnonymous]
        [HttpPost("v1/login/{roleId}")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto userLoginDto, int roleId)
        {
            IActionResult result = Ok();
            try
            {
                var userModel = await _authService.LoginUserByRoleId(userLoginDto, roleId);
                userModel.Role = new RoleModel() { Id = roleId };
                var userTokenModel = new TokenManager().GetTokenRefreshToken(userModel);
                await _authService.CreateRefreshToken(userTokenModel, userModel.Id);
                result = Ok(new ApiResponse<UserTokenDto>(userTokenModel, false, null, null, null));
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

        [AllowAnonymous]
        [HttpPost("v1/reset/password/{email}")]
        public async Task<IActionResult> SendEmailResetPasswordUser(string email)
        {
            IActionResult result = Ok();

            try
            {
                await _authService.SendEmailResetPasswordUser(email);
                result = StatusCode(200, new ApiResponse<string?>(null, false, null, null, null));
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

        [AllowAnonymous]
        [HttpPut("v1/reset/password")]
        public async Task<IActionResult> ResetPasswordUser([FromBody] ResetPasswordDto resetPassword)
        {
            IActionResult result = Ok();

            try
            {
                await _authService.ResetPasswordUser(resetPassword);
                result = StatusCode(200, new ApiResponse<string?>(null, false, "", null, null));
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

        [HttpPut("v1/token/refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto token)
        {
            IActionResult result = Ok();
            try
            {
                var userModel = await _authService.GetUserByRefreshToken(token);
                var tokenManager = new TokenManager();
                var newUserTokenDto = tokenManager.GetTokenRefreshToken(userModel);
                await _authService.UpdateRefreshToken(newUserTokenDto, token.RefreshToken, userModel.Id);
                result = Ok(new ApiResponse<UserTokenDto>(newUserTokenDto, false, null, null, null));
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

        [HttpPut("v1/validate/{token}")]
        public async Task<IActionResult> ValidateEmail(string token)
        {
            IActionResult result = Ok();

            try
            {
                await _authService.ValidateUserEmailByToken(token);
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

        [HttpGet("v1/verify/email/{email}")]
        public async Task<IActionResult> VerifyEmail(string email)
        {
            IActionResult result = Ok();

            try
            {
                await _authService.VerifyEmailExist(email);
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
