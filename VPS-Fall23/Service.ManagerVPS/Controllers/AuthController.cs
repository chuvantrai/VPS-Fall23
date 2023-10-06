using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.Input.User;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IGeneralVPS _generalVPS;

    public AuthController(IUserRepository userRepository, IGeneralVPS generalVps)
    {
        _userRepository = userRepository;
        _generalVPS = generalVps;
    }
    
    [HttpPost]
    public async Task<IActionResult> AuthLogin(LoginRequest request)
    {
        try
        {
            var account = await _userRepository.GetAccountByUserNameAsync(request.Username);
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
            var account = await _userRepository.GetAccountByIdAsync(Guid.Parse(userToken.UserId));
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
            _userRepository.RegisterNewAccount(newAccount);
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
        var account = await _userRepository.UpdateVerifyCodeAsync(request.UserName);
        if (account == null)
        {
            return BadRequest("wrong username!");
        }

        return Ok(account.Email);
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