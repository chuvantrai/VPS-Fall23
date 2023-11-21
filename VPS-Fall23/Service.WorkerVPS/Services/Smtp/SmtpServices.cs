using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using System.Text;
using Service.WorkerVPS.Models;

namespace Service.WorkerVPS.Services.Smtp
{
    internal class SmtpServices: ISmtpService
    {
        readonly string host, account, password;
        readonly int port;
        readonly bool useSsl;
        private readonly ILogger<SmtpServices> logger;

        public SmtpServices(ILogger<SmtpServices> logger,
            string host, string account, string password, int port, bool useSsl = false)
        {
            this.logger = logger;
            this.host = host;
            this.account = account;
            this.password = password;
            this.port = port;
            this.useSsl = useSsl;
        }
        public SmtpServices(IOptions<HostEmailProfile> hostEmailProfile, ILogger<SmtpServices> logger)
            : this(logger, hostEmailProfile.Value.Host, hostEmailProfile.Value.Account, hostEmailProfile.Value.Password, hostEmailProfile.Value.Port, hostEmailProfile.Value.UseSSL)
        {

        }
        public MailMessage CreateMailMessage(string subject, string body, params Attachment[]? attachments)
        {
            MailMessage mail = new()
            {
                From = new MailAddress(account),
                IsBodyHtml = true,
                BodyEncoding = UTF8Encoding.UTF8,
                Body = body,
                Subject = subject,

            };
            if (attachments != null && attachments.Length > 0)
            {
                foreach (var attachment in attachments)
                {
                    mail.Attachments.Add(attachment);
                }
            }
            return mail;
        }
        public MailMessage CreateMailMessage(string receiver, string subject, string body, params Attachment[]? attachments)
        {
            return CreateMailMessage(new string[1] { receiver }, subject, body, attachments);
        }
        public MailMessage CreateMailMessage(string[] receivers, string subject, string body, params Attachment[]? attachments)
        {
            MailMessage mail = CreateMailMessage(subject, body, attachments);
            foreach (var receiver in receivers)
            {
                mail.To.Add(receiver);
            }
            return mail;
        }
        public async Task Send(MailMessage mailMessage)
        {
            SmtpClient smtp = new SmtpClient(host, port)
            {
                DeliveryMethod = SmtpDeliveryMethod.Network,
                EnableSsl = useSsl,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(account, password),
            };
            logger.LogInformation($"SMTP Client has initiated: {smtp?.Host}, {smtp?.Port}");
            await Task.Run(() => smtp.Send(mailMessage));
            smtp.Dispose();
            logger.LogInformation($"SMTP Client has been disposed: {smtp?.Host}, {smtp?.Port}");

        }
        public async Task Send(string receiver, string subject, string body)
        {
            await Send(new string[1] { receiver }, subject, body);
        }

        public async Task Send(string[] receivers, string subject, string body)
        {
            MailMessage mailMessage = CreateMailMessage(receivers, subject, body);
            await Send(mailMessage);
        }
    }
}
