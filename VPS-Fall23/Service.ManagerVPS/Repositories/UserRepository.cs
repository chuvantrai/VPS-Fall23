using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
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
        var existAccount =
            context.Accounts.FirstOrDefault(x => x.Email.Equals(email) && x.IsVerified == true);
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

    public Account? GetOwnerAccountByEmail(string email)
    {
        var account = context.Accounts
            .Include(x => x.ParkingZoneOwner)
            .FirstOrDefault(x => x.Email.Equals(email));
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

    public async Task<Account?> UpdateVerifyCodeAsync(string userName, string? emailSubject = null)
    {
        emailSubject ??= "Verify Forgot password";

        var account = await context.Accounts.FirstOrDefaultAsync(x
            => (x.Username.Equals(userName) || x.Email.Equals(userName)) && !x.IsBlock &&
               x.TypeId != (int)UserRoleEnum.ATTENDANT);
        if (account == null) return null;

        if (account.ExpireVerifyCode == null || DateTime.Now > account.ExpireVerifyCode)
        {
            account.VerifyCode = _generalVPS.GenerateVerificationCode();
            account.ExpireVerifyCode = DateTime.Now.AddMinutes(30);
            await context.SaveChangesAsync();
            await _generalVPS.SendEmailAsync(account.Email,
                emailSubject,
                $"Your Verification code is: {account.VerifyCode}");
        }
        else
        {
            await _generalVPS.SendEmailAsync(account.Email,
                emailSubject,
                $"Your Verification code is: {account.VerifyCode}");
        }

        return account;
    }

    public async Task<Account?> ChangePasswordByUserIdAsync(Guid id, string password)
    {
        var account = await context.Accounts
            .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsBlock && x.IsVerified == true);
        if (account == null) return null;
        account.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password, 13);
        account.ModifiedAt = DateTime.Now;
        await context.SaveChangesAsync();
        return account;
    }

    public async Task<Account?> UpdateAccountById(UpdateProfileAccountRequest request)
    {
        var account = await context.Accounts
            .FirstOrDefaultAsync(x => x.Id.Equals(request.AccountId));
        if (account == null) return null;
        if (request.PathImage != null) account.Avatar = request.PathImage;
        account.FirstName = request.FirstName;
        account.LastName = request.LastName;
        account.PhoneNumber = request.PhoneNumber;
        if (request.Address != null) account.Address = request.Address;
        if (request.CommuneId is not null) account.CommuneId = request.CommuneId;
        account.ModifiedAt = DateTime.Now;
        await context.SaveChangesAsync();
        return account;
    }

    public async Task<Account?> GetAccountByIdAsync(Guid id)
    {
        var account = await context.Accounts
            .Include(x => x.Commune)
            .ThenInclude(x => x!.District)
            .ThenInclude(x => x.City)
            .Include(x => x.ParkingZoneOwner)
            .FirstOrDefaultAsync(x => x.Id.Equals(id) && !x.IsBlock && x.IsVerified == true);

        return account;
    }

    public Account? GetAccountToBlockById(Guid id)
    {
        var account = entities.FirstOrDefault(x => x.Id.Equals(id));
        return account;
    }

    public PagedList<Account> GetListAttendantAccount(Guid ownerId,
        QueryStringParameters parameters)
    {
        var attendants = entities
            .Include(x => x.ParkingZoneAttendant)
            .ThenInclude(x => x!.ParkingZone)
            .Where(x =>
                x.ParkingZoneAttendant!.ParkingZone.OwnerId.Equals(ownerId) &&
                x.TypeId == (int)UserRoleEnum.ATTENDANT);
        return PagedList<Account>.ToPagedList(attendants, parameters.PageNumber,
            parameters.PageSize);
    }

    public PagedList<Account> SearchAttendantByName(Guid ownerId, string attendantName,
        QueryStringParameters parameters)
    {
        var attendants = entities
            .Include(x => x.ParkingZoneAttendant)
            .ThenInclude(x => x!.ParkingZone)
            .Where(x =>
                x.ParkingZoneAttendant!.ParkingZone.OwnerId.ToString().Equals(ownerId) &&
                (x.FirstName.ToLower().Contains(attendantName) ||
                 x.LastName.ToLower().Contains(attendantName)) &&
                x.TypeId == (int)UserRoleEnum.ATTENDANT);
        return PagedList<Account>.ToPagedList(attendants, parameters.PageNumber,
            parameters.PageSize);
    }
}