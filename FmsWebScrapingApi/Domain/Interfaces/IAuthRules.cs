using System.ComponentModel.DataAnnotations;
using FmsWebScrapingApi.Domain.Entities;

namespace FmsWebScrapingApi.Domain.Interfaces
{
    public interface IAuthRules
    {
        public List<ValidationResult> ValidateUserLogin(UserLoginDto userLoginDto);
        public Task<List<ValidationResult>> ValidateCreateUser(UserRequest userRequest);
    }
}
