using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FALL23_SWP490_G14Context _context;

    public UserRepository(FALL23_SWP490_G14Context context)
    {
        _context = context;
    }

    public bool CheckEmailExists(string email)
    {
        var existAccount = _context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return existAccount is not null;
    }

    public bool CheckValidVerification(string email, int code)
    {
        var existedAcc = _context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return existedAcc?.VerifyCode == code;
    }

    public string AddUser()
    {
        return "Done";
    }

    public Account? GetAccountByEmail(string email)
    {
        var account = _context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return account;
    }

    public int RegisterNewAccount(Account newAccount)
    {
        _context.Accounts.Add(newAccount);
        var result = _context.SaveChanges();
        return result;
    }

    public void VerifyAccount(Account account)
    {
        account.IsVerified = true;
        _context.Accounts.Update(account);
        _context.SaveChanges();
    }
}