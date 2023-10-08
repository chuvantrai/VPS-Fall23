using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.Input.User;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class AuthController : VpsController<Account>
{
    private readonly IGeneralVPS _generalVps;

    public AuthController(IUserRepository userRepository, IGeneralVPS generalVps)
        : base(userRepository)
    {
        _generalVps = generalVps;
    }

    [HttpPost]
    public async Task<IActionResult> AuthLogin(LoginRequest request)
    {
        try
        {
            var account = await ((IUserRepository)vpsRepository).GetAccountByUserNameAsync(request.Username);
            if (account == null)
            {
                return BadRequest("wrong username!");
            }

            if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, account.Password))
            {
                return BadRequest("wrong password!");
            }

            if (account.IsVerified == false)
            {
                return BadRequest("Haven't Verified email yet!");
            }
        
            if (account.IsBlock)
            {
                return BadRequest("Account has been locked!");
            }

            var userToken = new UserTokenHeader
            {
                UserId = account.Id.ToString(),
                Email = account.Email,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Avatar = account.Avatar,
                RoleId = account.TypeId,
                RoleName = EnumExtension.CoverIntToEnum<UserRoleEnum>(account.TypeId).ToString(),
                Expires = DateTime.Now.AddMinutes(30),
                ModifiedAt = account.ModifiedAt
            };
            
            return Ok(new
            {
                AccessToken = JwtTokenExtension.WriteToken(userToken),
                UserData = userToken
            });
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        try
        {
            if (request.NewPassword == request.OldPassword)
            {
                return BadRequest("New password same old password!");
            }
            var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
            var userToken = JwtTokenExtension.ReadToken(accessToken)!;
            var account = await ((IUserRepository)vpsRepository).GetAccountByIdAsync(Guid.Parse(userToken.UserId));
            account!.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.NewPassword, 13);
            account.ModifiedAt = DateTime.Now;
            return Ok();
        }
        catch
        {
            return BadRequest();
        }
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.RefreshToken)]
    public IActionResult RefreshToken()
    {
        try
        {
            var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
            var userToken = JwtTokenExtension.ReadToken(accessToken)!;
            userToken.Expires = DateTime.Now.AddMinutes(30);
            return Ok(new
            {
                AccessToken = JwtTokenExtension.WriteToken(userToken),
                UserData = userToken
            });
        }
        catch
        {
            return BadRequest();
        }
    }
    
    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.CreateAccountDemo)]
    public IActionResult CreateAccountDemo([FromForm] CreateAccountDemoRequest request)
    {
        try
        {
            var newAccount = new Account()
            {
                TypeId = (int)request.TypeId,
                Id = Guid.NewGuid(),
                Username = request.Username,
                Email = request.Email,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(request.Password, 13),
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                IsBlock = false,
                PhoneNumber = request.PhoneNumber,
                IsVerified = true,
                VerifyCode = 0
            };
            // ((IUserRepository)vpsRepository).RegisterNewAccount(newAccount);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut]
    public async Task<IActionResult> SendCodeForgotPassword(SendCodeForgotPasswordRequest request)
    {
        var account = await ((IUserRepository)vpsRepository).UpdateVerifyCodeAsync(request.UserName);
        if (account == null)
        {
            return BadRequest("wrong username!");
        }

        return Ok(account.Email);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterAccount input)
    {
        var isExisting = ((IUserRepository)vpsRepository).CheckEmailExists(input.Email);
        if (isExisting)
        {
            throw new ClientException(6);
        }

        var verifyCode = _generalVps.GenerateVerificationCode();

        var newAccount = new Account
        {
            TypeId = (int)UserRoleEnum.OWNER,
            Id = Guid.NewGuid(),
            Email = input.Email,
            Username = input.Email,
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(input.Password, 13),
            FirstName = input.FirstName,
            LastName = input.LastName,
            PhoneNumber = input.PhoneNumber,
            IsBlock = false,
            IsVerified = false,
            VerifyCode = verifyCode,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            ExpireVerifyCode = DateTime.Now.AddHours(1)
        };

        var result = await ((IUserRepository)vpsRepository).Create(newAccount);
        if (result is null)
        {
            throw new ServerException(ResponseNotification.ADD_ERROR);
        }

        await ((IUserRepository)vpsRepository).SaveChange();

        await _generalVps.SendEmailAsync(input.Email,
            "Verify Your Email",
            $"Your Verification code is: {verifyCode}");

        return Ok(ResponseNotification.ADD_SUCCESS);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyNewAccount([FromBody] ValidateNewAccount input)
    {
        var account = ((IUserRepository)vpsRepository).GetAccountByEmail(input.Email);
        if (account is null) throw new ClientException("Your Email is not registered!");
        
        if(account.ExpireVerifyCode < DateTime.Now) throw new ClientException("Verify Code is expired!");

        var isValidCode = ((IUserRepository)vpsRepository).CheckValidVerification(input.Email, input.VerifyCode);
        if (!isValidCode) throw new ClientException("Verify Code is not Valid! Please Try Again!");

        ((IUserRepository)vpsRepository).VerifyAccount(account);
        await ((IUserRepository)vpsRepository).SaveChange();

        return Ok("Verify success!");
    }
}