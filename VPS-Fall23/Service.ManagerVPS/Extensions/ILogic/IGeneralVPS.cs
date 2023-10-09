namespace Service.ManagerVPS.Extensions.ILogic;

public interface IGeneralVPS
{
    Task<bool> SendEmailAsync(string recipient, string subject, string body);

    int GenerateVerificationCode();
}