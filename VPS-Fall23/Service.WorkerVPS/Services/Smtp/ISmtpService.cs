using System.Net.Mail;

namespace Service.WorkerVPS.Services.Smtp
{
    internal interface ISmtpService
    {
        MailMessage CreateMailMessage(string subject, string body, params Attachment[]? attachments);
        MailMessage CreateMailMessage(string receiver, string subject, string body, params Attachment[]? attachments);
        MailMessage CreateMailMessage(string[] receivers, string subject, string body, params Attachment[]? attachments);
        Task Send(MailMessage mailMessage);
        Task Send(string receiver, string subject, string body);
        Task Send(string[] receivers, string subject, string body);
    }
}
