using FmsWebScrapingApi.Domain.Interfaces;

namespace FmsWebScrapingApi.Domain.Entities
{
    public class ApiResponse<T> : IApiResponse<T>
    {
        public T Data { get; set; }
        public bool Error { get; set; }
        public string? Message { get; set; }
        public int? Page { get; set; }
        public int? TotalPage { get; set; }

        public ApiResponse(T data, bool error, string? message, int? page, int? totalPage)
        {
            Data = data;
            Error = error;
            Message = message;
            Page = page;
            TotalPage = totalPage;
        }
    }
}
