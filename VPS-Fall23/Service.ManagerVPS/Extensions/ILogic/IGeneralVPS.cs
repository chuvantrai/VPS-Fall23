using Service.ManagerVPS.DTO.OtherModels;

namespace Service.ManagerVPS.Extensions.ILogic;

public interface IGeneralVPS
{
    Task<bool> SendEmailAsync(string recipient, string subject, string body);

    int GenerateVerificationCode();

    Task<bool> SendListEmailAsync(IEnumerable<string> recipients, string subject, string body);

    string CreateTemplateEmail(List<KeyValue> keyValues);
}