using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using FmsWebScrapingApi.Data.Interfaces;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Domain.Interfaces;
using FmsWebScrapingApi.Infra.Constants;

namespace FmsWebScrapingApi.Domain.Rules
{
    public class AuthRules : IAuthRules
    {
        private IUserRepository _userRepository;

        public AuthRules(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<ValidationResult> ValidateUserLogin(UserLoginDto userLoginDto)
        {
            var listValidationErrors = new List<ValidationResult>();
            ValidationResult validationName = ValidateEmail(userLoginDto.Email);
            if (validationName != ValidationResult.Success)
            {
                listValidationErrors.Add(validationName);
            }

            ValidationResult validationSurname = ValidatePassword(userLoginDto.Password);
            if (validationSurname != ValidationResult.Success)
            {
                listValidationErrors.Add(validationSurname);
            }

            return listValidationErrors;
        }

        public ValidationResult ValidateEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult("O email de usuário é obrigatório.");
            }

            if (email.Length > 254)
            {
                return new ValidationResult("O email do não pode conter mais de 254 caracteres.");
            }

            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!regex.IsMatch(email))
            {
                return new ValidationResult("Formato de e-mail incorreto.");
            }

            return ValidationResult.Success!;
        }

        private ValidationResult ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return new ValidationResult("A senha de usuário não pode ser vazia ou nula.");
            }

            string regStr = "";
            regStr += UserConstants.PASSWORD_SHOULD_HAS_NUMBERS ? "(?=.*\\d)" : "";
            regStr += UserConstants.PASSWORD_SHOULD_HAS_LOWER ? "(?=.*[a-z])" : "";
            regStr += UserConstants.PASSWORD_SHOULD_HAS_UPPER ? "(?=.*[A-Z])" : "";
            regStr += UserConstants.PASSWORD_SHOULD_HAS_SYMBOLS ? "(?=.*[$*&@#])" : "";
            regStr += $"[0-9a-zA-Z$*&@#]{{{UserConstants.PASSWORD_MINSIZE},}}";
            bool diag = Regex.IsMatch(password, regStr);
            diag = true;
            diag = password.Length <= UserConstants.PASSWORD_MAXSIZE && diag;

            if (!diag)
            {
                return new ValidationResult("O formado de senha não é válido.");
            }

            return ValidationResult.Success!;
        }

        public async Task<List<ValidationResult>> ValidateCreateUser(UserRequest userRequest)
        {
            var listValidationErrors = new List<ValidationResult>();

            try
            {
                var userModel = await _userRepository.GetUserByEmail(userRequest.Email);
                if (userModel != null)
                {
                    listValidationErrors.Add(new ValidationResult("O E-mail já existe em nossa base de dados."));
                }
            }
            catch (ApiException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new ApiException(ErrorMessageConstants.GenericError, ErrorCodeConstants.GenericError, ex.Data);
            }

            return listValidationErrors;
        }
    }
}
