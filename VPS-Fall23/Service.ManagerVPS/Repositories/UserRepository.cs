using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class UserRepository : VpsRepository<Account>, IUserRepository
{
    private readonly IGeneralVPS _generalVPS; 
    public UserRepository(FALL23_SWP490_G14Context context, IGeneralVPS generalVps)
        : base(context)
    {
        _generalVPS = generalVps;
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

    public async Task<Account?> GetAccountByUserNameAsync(string userName)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(userName));
        return account;
    }

    public async Task<Account?> GetAccountByIdAsync(Guid id)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Id.Equals(id));
        return account;
    }

    public async Task<Account?> UpdateVerifyCodeAsync(string userName)
    {
        var account = await context.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(userName));
        if (account == null) return null;
        account.VerifyCode = _generalVPS.GenerateVerificationCode();
        await context.SaveChangesAsync();
        await _generalVPS.SendEmailAsync(account.Email,
            "Verify Forgot password",
            $"Your Verification code is: {account.VerifyCode}");
        return account;
    }
}