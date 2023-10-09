using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FALL23_SWP490_G14Context _context;
    private readonly IGeneralVPS _generalVPS;

    public UserRepository(FALL23_SWP490_G14Context context, IGeneralVPS generalVPS)
    {
        _context = context;
        _generalVPS = generalVPS;
    }

    public bool CheckEmailExists(string email)
    {
        var existAccount = _context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return existAccount is not null;
    }

    public bool CheckValidVerification(string email, int code)
    {
        var existedAcc = _context.Accounts.FirstOrDefault(x => x.Email.Equals(email));
        return existedAcc.VerifyCode == code;
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

    public async Task<Account?> GetAccountByUserNameAsync(string userName)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Username.Equals(userName));
        return account;
    }

    public async Task<Account?> GetAccountByIdAsync(Guid id)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsBlock);
        return account;
    }

    public async Task<Account?> UpdateVerifyCodeAsync(string userName)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x
            => x.Username.Equals(userName) && !x.IsBlock && x.TypeId != (int)UserRoleEnum.ATTENDANT);
        if (account == null) return null;
        if (account.ExpireVerifyCode == null || DateTime.Now > account.ExpireVerifyCode)
        {
            account.VerifyCode = _generalVPS.GenerateVerificationCode();
            account.ExpireVerifyCode = DateTime.Now.AddHours(1);
            await _context.SaveChangesAsync();
            await _generalVPS.SendEmailAsync(account.Email,
                "Verify Forgot password",
                $"Your Verification code is: {account.VerifyCode}");
        }
        else
        {
            await _generalVPS.SendEmailAsync(account.Email,
                "Verify Forgot password",
                $"Your Verification code is: {account.VerifyCode}");
        }
        
        return account;
    }

    public async Task<Account?> ChangePasswordByUserIdAsync(Guid id, string password)
    {
        var account = await _context.Accounts.FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsBlock);
        if (account == null) return null;
        account.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        account.ModifiedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return account;
    }
}