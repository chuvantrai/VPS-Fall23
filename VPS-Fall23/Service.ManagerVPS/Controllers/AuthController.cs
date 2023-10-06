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

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : VpsController<Account>
{
    private readonly FALL23_SWP490_G14Context _context;
    private readonly IUserRepository _userRepository;
    private readonly IGeneralVPS _generalVps;

    public AuthController(FALL23_SWP490_G14Context context, IUserRepository userRepository, IGeneralVPS generalVps)
        : base(userRepository)
    {
        _context = context;
        _userRepository = userRepository;
        _generalVps = generalVps;
    }

    [HttpPost("AuthLogin")]
    public async Task<IActionResult> AuthLogin()
    {
        return Ok();
    }

    [HttpPost("CreateAccountDemo")]
    public async Task<IActionResult> CreateAccountDemo([FromForm] CreateAccountDemoRegister request)
    {
        var newAccount = new Account()
        {
            TypeId = request.TypeId,
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
            //Message này cho ra constant nhé
            throw new ClientException("Email already exists! Try to use another email address!");
        }

        var verifyCode = _generalVps.GenerateVerificationCode();

        //Những thuộc tính mặc định luôn luôn có lúc khởi tạo thì cho vô contructor đi nha
        var newAccount = new Account
        {
            TypeId = UserRoleEnum.OWNER,
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

        var result = _userRepository.RegisterNewAccount(newAccount);
        if (result == 0)
        {
            throw new ServerException(ResponseNotification.ADD_ERROR);
        }

        //Cái này cho ra 1 function trong user repo nhé
        await _generalVps.SendEmailAsync(input.Email,
            "Verify Your Email",
            $"Your Verification code is: {verifyCode}");

        return Ok(ResponseNotification.ADD_SUCCESS);

    }

    [HttpPost]
    public IActionResult VerifyNewAccount([FromBody] ValidateNewAccount input)
    {
        var account = _userRepository.GetAccountByEmail(input.Email);
        if (account is null) return BadRequest("Your Email is not registered!");

        var isValidCode = _userRepository.CheckValidVerification(input.Email, input.VerifyCode);
        if (!isValidCode) return BadRequest("Verify Code is not Valid! Please Try Again!");

        _userRepository.VerifyAccount(account);

        return Ok("Verify success!");
    }
}