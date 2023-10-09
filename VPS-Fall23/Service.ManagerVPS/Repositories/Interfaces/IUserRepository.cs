using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IUserRepository
{
    bool CheckEmailExists(string email);

    bool CheckValidVerification(string email, int code);
    
    string AddUser();
    
    Account? GetAccountByEmail(string email);

    int RegisterNewAccount(Account newAccount);
    
    void VerifyAccount(Account account);
    
    Task<Account?> GetAccountByUserNameAsync(string userName);
    
    Task<Account?> GetAccountByIdAsync(Guid id);
    
    Task<Account?> UpdateVerifyCodeAsync(string userName);
    
    Task<Account?> ChangePasswordByUserIdAsync(Guid id, string password);
}