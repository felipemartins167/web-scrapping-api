using FmsWebScrapingApi.Domain.Interfaces;

namespace FmsWebScrapingApi.Domain.Entities
{
    public class ApiException : Exception, IApiException
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public dynamic? ErrorData { get; set; }

        public ApiException(string errorMessage, string errorCode, dynamic? errorData)
        {
            ErrorMessage = errorMessage;
            ErrorCode = errorCode;
            ErrorData = errorData;
        }


    }
}
