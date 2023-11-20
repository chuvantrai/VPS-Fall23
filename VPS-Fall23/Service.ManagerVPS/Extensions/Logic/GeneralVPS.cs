using System.Net;
using System.Net.Mail;
using System.Text;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;

namespace Service.ManagerVPS.Extensions.Logic;

public class GeneralVPS : IGeneralVPS
{
    public async Task<bool> SendEmailAsync(string recipient, string subject, string body)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        using (var client = new SmtpClient("smtp.gmail.com", 587))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(config["EmailSetting:Email"],
                config["EmailSetting:Password"]);

            var message = new MailMessage
            {
                From = new MailAddress(config["EmailSetting:Email"]),
                To = { recipient },
                Subject = subject,
                Body = body
            };
            message.BodyEncoding = Encoding.UTF8;
            message.SubjectEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            message.ReplyToList.Add(new MailAddress(config["EmailSetting:Email"]));
            message.Sender = new MailAddress(config["EmailSetting:Email"]);

            try
            {
                await client.SendMailAsync(message);
                return true;
            }
            catch (SmtpException ex)
            {
                // Lỗi xảy ra khi gửi email
                Console.WriteLine("Error: " + ex.Message);
                return false;
            }
        }
    }

    public async Task<bool> SendListEmailAsync(IEnumerable<string> recipients, string subject,
        string body)
    {
        try
        {
            var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
            using var client = new SmtpClient("smtp.gmail.com", 587);
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(config["EmailSetting:Email"],
                config["EmailSetting:Password"]);

            var message = new MailMessage
            {
                From = new MailAddress(config["EmailSetting:Email"]),
                Subject = subject,
                Body = body,
                BodyEncoding = Encoding.UTF8,
                SubjectEncoding = Encoding.UTF8,
                IsBodyHtml = true,
                Sender = new MailAddress(config["EmailSetting:Email"])
            };
            message.ReplyToList.Add(new MailAddress(config["EmailSetting:Email"]));
            foreach (var recipient in recipients)
            {
                message.To.Add(recipient);
            }

            await client.SendMailAsync(message);
            return true;
        }
        catch (SmtpException ex)
        {
            // Lỗi xảy ra khi gửi email
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }

    public int GenerateVerificationCode()
    {
        var rnd = new Random();
        return rnd.Next(100000, 999999);
    }

    public string CreateTemplateEmail(IEnumerable<KeyValue> keyValues, string fileName)
    {
        var filePath = Path.Combine(Directory.GetCurrentDirectory(),
            "Constants", "FileHtml", fileName);
        var htmlContent = File.ReadAllText(filePath);
        return keyValues.Aggregate(htmlContent,
            (current, keyValue) => current.Replace(keyValue.Key, keyValue.Value));
    }
}