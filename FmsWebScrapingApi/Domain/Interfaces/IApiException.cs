namespace FmsWebScrapingApi.Domain.Interfaces
{
    public interface IApiException
    {
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public dynamic? ErrorData { get; set; }
    }
}
