using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Domain.Interfaces;
using FmsWebScrapingApi.Infra.Config;
using FmsWebScrapingApi.Infra.Constants;
using Microsoft.IdentityModel.Tokens;

namespace FmsWebScrapingApi.Infra.Helpers.Encrypt
{
    public class TokenManager
    {
        private IConfiguration _config;

        public TokenManager()
        {
            _config = AppSettingsConfig.GetConfiguration();
        }

        public string GenerateJSONWebToken(IUserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.Name),
                new Claim("role", userInfo.Role.Id.ToString()),
                new Claim("id", userInfo.Id.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.UtcNow.AddDays(Convert.ToInt32(_config["Jwt:ExpirationMinutes"])),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public ClaimsPrincipal DecodeJwtToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidIssuer = _config["Jwt:Issuer"],
                ValidAudience = _config["Jwt:Audience"]
            };

            try
            {
                ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                return claimsPrincipal;
            }
            catch (SecurityTokenException e)
            {
                throw new ApiException(ErrorMessageConstants.SecurityInvalidToken, ErrorCodeConstants.SecurityInvalidToken, e);
            }
        }

        public string GenerateCustomToken(int length)
        {
            var randomNumber = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public string GenerateCustomTokenUrl(int length)
        {
            var randomNumber = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                string base64Token = Convert.ToBase64String(randomNumber);
                string urlEncodedToken = HttpUtility.UrlEncode(base64Token);
                return urlEncodedToken;
            }
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public UserTokenDto GetTokenRefreshToken(IUserModel user)
        {
            UserTokenDto userTokenModel = new UserTokenDto();
            userTokenModel.Token = new TokenManager().GenerateJSONWebToken(user);
            userTokenModel.RefreshToken = new TokenManager().GenerateRefreshToken();
            return userTokenModel;
        }
    }
}
