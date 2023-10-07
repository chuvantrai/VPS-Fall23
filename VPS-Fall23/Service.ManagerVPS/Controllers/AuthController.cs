using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.Input.User;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class AuthController : VpsController<Account>
{
    private readonly IUserRepository _userRepository;
    private readonly IGeneralVPS _generalVps;

    public AuthController(IUserRepository userRepository, IGeneralVPS generalVps)
        : base(userRepository)
    {
        _userRepository = userRepository;
        _generalVps = generalVps;
    }

    [HttpPost]
    public async Task<IActionResult> AuthLogin()
    {
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> CreateAccountDemo([FromForm] CreateAccountDemoRegister request)
    {
        var newAccount = new Account()
        {
            TypeId = (int)request.TypeId,
        };
        Account account = await this.vpsRepository.Create(newAccount);
        await this.vpsRepository.SaveChange();
        return Ok(account);
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterAccount input)
    {
        var isExisting = _userRepository.CheckEmailExists(input.Email);
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
        };

        var result = await _userRepository.Create(newAccount);
        if (result is null)
        {
            throw new ServerException(ResponseNotification.ADD_ERROR);
        }

        await _userRepository.SaveChange();

        await _generalVps.SendEmailAsync(input.Email,
            "Verify Your Email",
            $"Your Verification code is: {verifyCode}");

        return Ok(ResponseNotification.ADD_SUCCESS);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyNewAccount([FromBody] ValidateNewAccount input)
    {
        var account = _userRepository.GetAccountByEmail(input.Email);
        if (account is null) return BadRequest("Your Email is not registered!");

        var isValidCode = _userRepository.CheckValidVerification(input.Email, input.VerifyCode);
        if (!isValidCode) return BadRequest("Verify Code is not Valid! Please Try Again!");

        _userRepository.VerifyAccount(account);
        await _userRepository.SaveChange();

        return Ok("Verify success!");
    }
}