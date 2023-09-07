using Service.ManagerCRM.Extensions.ILogic;
using Service.ManagerCRM.ModelsNoneDB;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Service.ManagerCRM.Extensions.Logic;

public class GeneralCRM : IGeneralCRM
{
    public async Task<bool> SendEmailAsync(string recipient, string subject, string body)
    {
        var config = new ConfigurationBuilder().AddJsonFile("appsettings.Development.json").Build();
        using (var client = new SmtpClient("smtp.gmail.com", 587))
        {
            client.EnableSsl = true;
            client.Credentials = new NetworkCredential(config["EmailSetting:Email"], config["EmailSetting:Password"]);

            var message = new MailMessage
            {
                From = new MailAddress(config["EmailSetting:Email"]),
                To = { recipient },
                Subject = subject,
                Body = body
            };
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.SubjectEncoding = System.Text.Encoding.UTF8;
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
}