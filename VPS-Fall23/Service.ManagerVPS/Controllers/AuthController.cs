using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.Input.User;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly FALL23_SWP490_G14Context _context;
    private readonly IUserRepository _userRepository;
    private readonly IGeneralVPS _generalVPS;

    public AuthController(FALL23_SWP490_G14Context context, IUserRepository userRepository, IGeneralVPS generalVps)
    {
        _context = context;
        _userRepository = userRepository;
        _generalVPS = generalVps;
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
            TypeId = (int)request.TypeId,
        };
        _context.Accounts.Add(newAccount);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterAccount input)
    {
        try
        {
            var isExisting = _userRepository.CheckEmailExists(input.Email);
            if (isExisting)
            {
                return BadRequest("Email already exists! Try to use another email address!");
            }

            var verifyCode = _generalVPS.GenerateVerificationCode();
            var newAccount = new Account
            {
                TypeId = (int)UserRoleEnum.OWNER,
                Id = new Guid(),
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
                return BadRequest(ResponseNotification.ADD_ERROR);
            }

            await _generalVPS.SendEmailAsync(input.Email,
                "Verify Your Email",
                $"Your Verification code is: {verifyCode}");

            return Ok(ResponseNotification.ADD_SUCCESS);
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }

    [HttpPost]
    public IActionResult VerifyNewAccount([FromBody] ValidateNewAccount input)
    {
        try
        {
            var account = _userRepository.GetAccountByEmail(input.Email);
            if (account is null) return BadRequest("Your Email is not registered!");

            var isValidCode = _userRepository.CheckValidVerification(input.Email, input.VerifyCode);
            if (!isValidCode) return BadRequest("Verify Code is not Valid! Please Try Again!");

            _userRepository.VerifyAccount(account);

            return Ok("Verify success!");
        }
        catch (Exception ex)
        {
            return BadRequest(ex);
        }
    }
}