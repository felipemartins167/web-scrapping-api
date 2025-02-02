using System.Net;
using System.Net.Mail;
using FmsWebScrapingApi.Domain.Entities;
using FmsWebScrapingApi.Infra.Config;
using FmsWebScrapingApi.Infra.Constants;

namespace EstacaoDoOlhoApi.Infra.Helpers.Email
{
    public static class EmailHelper
    {

        public static void AddMailsToMailMessage(MailMessage mailMessage, string[] emails)
        {
            foreach (var email in emails)
            {
                mailMessage.To.Add(email);
            }
        }
        public static async Task<bool> SendMail(string[] emails, string body, string subject)
        {
            try
            {
                IConfiguration configuration = AppSettingsConfig.GetConfiguration();
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(configuration["MailData:emailSender"]!);
                AddMailsToMailMessage(mailMessage, emails);
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                mailMessage.IsBodyHtml = true;
                SmtpClient smtpClient = new SmtpClient(configuration["MailData:smtpAddress"]!, Convert.ToInt32(configuration["MailData:portNumber"]!));
                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(configuration["MailData:emailSender"]!, configuration["MailData:password"]!);
                smtpClient.EnableSsl = true;
                await smtpClient.SendMailAsync(mailMessage);
            }
            catch (Exception e)
            {
                throw new ApiException(ErrorMessageConstants.SendEmail, ErrorCodeConstants.SendEmail, e);
            }
            return true;

        }
    }
}
