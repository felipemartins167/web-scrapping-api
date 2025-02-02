namespace FmsWebScrapingApi.Domain.Interfaces
{
    public interface IApiResponse<T>
    {
        public T Data { get; set; }
        public bool Error { get; set; }
        public string? Message { get; set; }
    }
}
