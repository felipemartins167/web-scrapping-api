namespace FmsWebScrapingApi.Data.Repository.EmailRepositoryHandler
{
    public static class EmailSqlStatments
    {
        public static string GetEmailTemplateByTemplateIdentifier()
        {
            return $"SELECT id, message, template_identifier, create_at, update_at FROM email_templates where template_identifier = @templateId";
        }
    }
}
