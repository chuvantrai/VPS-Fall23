using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.Models;

namespace Service.ManagerVPS.Repositories.Interfaces;

public interface IUserRepository : IVpsRepository<Account>
{
    bool CheckEmailExists(string email);

    bool CheckValidVerification(string email, int code);
    
    string AddUser();
    
    Account? GetAccountByEmail(string email);

    void VerifyAccount(Account account);
    
    Task<Account?> GetAccountByUserNameAsync(string userName);
    
    Task<Account?> GetAccountByIdAsync(Guid id);
    
    Task<Account?> UpdateVerifyCodeAsync(string userName);
    
    Task<Account?> UpdateAccountById(UpdateProfileAccountRequest request);
}