using Service.ManagerCRM.ModelsNoneDB;

namespace Service.ManagerCRM.Extensions.ILogic;

public interface IGeneralCRM
{
     Task<bool> SendEmailAsync(string recipient, string subject, string body);
}