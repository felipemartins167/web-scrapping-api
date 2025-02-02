namespace FmsWebScrapingApi.Data.Interfaces
{
    public interface IEmailTemplate
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public string TemplateIdentifier { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
