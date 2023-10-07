using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class UserRepository : VpsRepository<Account>, IUserRepository
{
    public UserRepository(FALL23_SWP490_G14Context context)
        : base(context)
    {
    }

    public bool CheckEmailExists(string email)
    {
        var existAccount = context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return existAccount is not null;
    }

    public bool CheckValidVerification(string email, int code)
    {
        var existedAcc = context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return existedAcc?.VerifyCode == code;
    }

    public string AddUser()
    {
        return "Done";
    }

    public Account? GetAccountByEmail(string email)
    {
        var account = context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return account;
    }

    public void VerifyAccount(Account account)
    {
        account.IsVerified = true;
        Update(account);
    }
}