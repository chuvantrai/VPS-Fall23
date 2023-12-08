using Service.ManagerVPS.DTO.OtherModels;

namespace Service.ManagerVPS.Extensions.ILogic;

public interface IGeneralVPS
{
    Task<bool> SendEmailAsync(string recipient, string subject, string body);

    int GenerateVerificationCode();
    
    string GenerateRandomCode(int length);

    Task<bool> SendListEmailAsync(IEnumerable<string> recipients, string subject, string body);

    string CreateTemplateEmail(IEnumerable<KeyValue> keyValues);
    
    string CreateTemplateEmail(IEnumerable<KeyValue> keyValues, string fileName);
}