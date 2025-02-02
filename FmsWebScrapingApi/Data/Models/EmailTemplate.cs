using FmsWebScrapingApi.Data.Interfaces;

namespace FmsWebScrapingApi.Data.Models
{
    public class EmailTemplate : IEmailTemplate
    {
        public int Id { get; set; }

        public required string Message { get; set; }

        public required string TemplateIdentifier { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
