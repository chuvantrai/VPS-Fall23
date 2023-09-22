namespace Service.ManagerVPS.Extensions.ILogic;

public interface IGeneralCRM
{
    Task<bool> SendEmailAsync(string recipient, string subject, string body);
}