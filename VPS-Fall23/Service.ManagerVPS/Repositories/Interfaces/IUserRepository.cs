
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IUserRepository : IVpsRepository<Account>
{
    bool CheckEmailExists(string email);

    bool CheckValidVerification(string email, int code);
    
    string AddUser();
    
    Account? GetAccountByEmail(string email);
    
    Account? GetOwnerAccountByEmail(string email);

    void VerifyAccount(Account account);
    
    Task<Account?> GetAccountByUserNameAsync(string userName);

    Task<Account?> UpdateVerifyCodeAsync(string userName, string? emailSubject = null);
    
    Task<Account?> ChangePasswordByUserIdAsync(Guid id, string password);
    
    Task<Account?> UpdateAccountById(UpdateProfileAccountRequest request);
}