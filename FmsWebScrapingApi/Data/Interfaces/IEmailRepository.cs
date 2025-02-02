using FmsWebScrapingApi.Data.Context;

namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IEmailRepository : IBaseRepository
    {
        public Task<IEmailTemplate>? GetEmailTemplateByTemplateIdentifier(string templateIdentifier);
    }
}
