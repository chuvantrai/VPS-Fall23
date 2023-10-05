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
}