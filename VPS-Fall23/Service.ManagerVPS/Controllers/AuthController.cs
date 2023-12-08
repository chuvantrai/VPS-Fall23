using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.DTO.Output;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class AuthController : VpsController<Account>
{
    private readonly IGeneralVPS _generalVps;
    private readonly IParkingZoneOwnerRepository _parkingZoneOwnerRepository;
    private readonly IConfiguration _config;
    private readonly FileManagementConfig _fileManagementConfig;

    public AuthController(IUserRepository userRepository, IGeneralVPS generalVps,
        IParkingZoneOwnerRepository parkingZoneOwnerRepository, IConfiguration config,
        IOptions<FileManagementConfig> options)
        : base(userRepository)
    {
        _generalVps = generalVps;
        _parkingZoneOwnerRepository = parkingZoneOwnerRepository;
        _config = config;
        _fileManagementConfig = options.Value;
    }

    [HttpPost]
    public async Task<IActionResult> AuthLogin(LoginRequest request)
    {
        var account =
            await ((IUserRepository)vpsRepository).GetAccountByUserNameAsync(request.Username);
        if (account == null)
        {
            throw new ClientException(5001);
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, account.Password))
        {
            throw new ClientException(5006);
        }

        if (account.IsVerified == false)
        {
            throw new ClientException(5007);
        }

        if (account.IsBlock)
        {
            throw new ClientException(5002);
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

    [HttpPut]
    //[FilterPermission(Action = ActionFilterEnum.ChangePassword)]
    public async Task<IActionResult> ChangePassword(ChangePasswordRequest request)
    {
        if (request.NewPassword == request.OldPassword)
        {
            throw new ClientException(5005);
        }

        var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
        var userToken = JwtTokenExtension.ReadToken(accessToken)!;
        var oldAccount =
            await ((IUserRepository)vpsRepository).GetAccountByIdAsync(
                Guid.Parse(userToken.UserId));
        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.OldPassword, oldAccount?.Password))
        {
            throw new ClientException(5006);
        }

        var account =
            await ((IUserRepository)vpsRepository).ChangePasswordByUserIdAsync(
                Guid.Parse(userToken.UserId),
                request.NewPassword);
        if (account == null) throw new ClientException();
        return Ok();
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.RefreshToken)]
    public IActionResult RefreshToken()
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
    public async Task<IActionResult> ResendVerificationCode(SendCodeForgotPasswordRequest request)
    {
        var account =
            await ((IUserRepository)vpsRepository).UpdateVerifyCodeAsync(request.UserName);
        if (account == null)
        {
            throw new ClientException("Tên tài khoản/email không chính xác!");
        }

        return Ok(account.Email);
    }

    [HttpPut]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordRequest request)
    {
        var account =
            await ((IUserRepository)vpsRepository).GetAccountByUserNameAsync(request.UserName);
        if (account == null) throw new ClientException(5001);
        if (account.IsBlock)
        {
            throw new ClientException(5002);
        }

        if (account.VerifyCode != request.VerifyCode)
        {
            throw new ClientException(5003);
        }

        if (DateTime.Now > account.ExpireVerifyCode)
        {
            throw new ClientException(5004);
        }

        var accountAfterChange =
            await ((IUserRepository)vpsRepository).ChangePasswordByUserIdAsync(account.Id,
                request.Password);
        if (accountAfterChange == null) throw new ClientException();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromBody] RegisterAccount input)
    {
        var existingAccount = ((IUserRepository)vpsRepository).GetOwnerAccountByEmail(input.Email);
        var verifyCode = _generalVps.GenerateVerificationCode();

        if (existingAccount is not null)
        {
            if (existingAccount.IsVerified == true)
            {
                throw new ClientException(6);
            }

            existingAccount.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(input.Password, 13);
            existingAccount.FirstName = input.FirstName;
            existingAccount.LastName = input.LastName;
            existingAccount.PhoneNumber = input.PhoneNumber;
            existingAccount.VerifyCode = verifyCode;
            existingAccount.ExpireVerifyCode = DateTime.Now.AddMinutes(30);
            await ((IUserRepository)vpsRepository).Update(existingAccount);

            var parkingZoneOwnerExistedAccount = existingAccount.ParkingZoneOwner!;
            parkingZoneOwnerExistedAccount.Phone = input.PhoneNumber;
            parkingZoneOwnerExistedAccount.Dob = input.Dob;
            await _parkingZoneOwnerRepository.Update(parkingZoneOwnerExistedAccount);
        }
        else
        {
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
                ExpireVerifyCode = DateTime.Now.AddMinutes(30)
            };
            var result = await ((IUserRepository)vpsRepository).Create(newAccount);
            if (result is null)
            {
                throw new ServerException(ResponseNotification.ADD_ERROR);
            }

            var parkingZoneOwnerRec = new ParkingZoneOwner
            {
                Id = newAccount.Id,
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
                Phone = input.PhoneNumber,
                Email = input.Email,
                Dob = input.Dob,
            };
            var parkingZoneOwnerResult =
                await _parkingZoneOwnerRepository.Create(parkingZoneOwnerRec);
            if (parkingZoneOwnerResult is null)
            {
                throw new ServerException(ResponseNotification.ADD_ERROR);
            }
        }

        await ((IUserRepository)vpsRepository).SaveChange();

        await _generalVps.SendEmailAsync(input.Email,
            "Xác thực tài khoản",
            $"Mã xác thực tài khoản của bạn là: {verifyCode}");

        return Ok(ResponseNotification.ADD_SUCCESS);
    }

    [HttpPost]
    public async Task<IActionResult> VerifyNewAccount([FromBody] ValidateNewAccount input)
    {
        var account = ((IUserRepository)vpsRepository).GetAccountByEmail(input.Email);
        if (account is null) throw new ClientException(2003);

        if (account.ExpireVerifyCode < DateTime.Now) throw new ClientException(2002);

        var isValidCode =
            ((IUserRepository)vpsRepository).CheckValidVerification(input.Email, input.VerifyCode);
        if (!isValidCode) throw new ClientException(2001);

        ((IUserRepository)vpsRepository).VerifyAccount(account);
        await ((IUserRepository)vpsRepository).SaveChange();

        return Ok("Xác thực tài khoản thành công!");
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.UpdateProfileAccount)]
    public async Task<IActionResult> UpdateProfileAccount(
        [FromForm] UpdateProfileAccountRequest request)
    {
        var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
        var userToken = JwtTokenExtension.ReadToken(accessToken)!;
        request.AccountId = Guid.Parse(userToken.UserId);
        // saveFile 
        if (request.AvatarImages?[0] != null)
        {
            var fileManager =
                new FileManagementClient(
                    _config.GetValue<string>("fileManagementAccessKey:baseUrl"),
                    _config.GetValue<string>("fileManagementAccessKey:accessKey"),
                    _config.GetValue<string>("fileManagementAccessKey:secretKey"));
            var avatarImg = new MultipartFormDataContent();

            var streamContent = new StreamContent(request.AvatarImages[0].OpenReadStream());
            avatarImg.Add(streamContent, FileManagementClient.MULTIPART_FORM_PARAM_NAME,
                $"{Guid.Parse(userToken.UserId)}-{0}{Path.GetExtension(request.AvatarImages[0].FileName)}");

            request.PathImage =
                $"{_fileManagementConfig.EndPointServer}:{_fileManagementConfig.EndPointPort.Api}/" +
                $"{_config.GetValue<string>("fileManagementAccessKey:publicBucket")}" +
                $"/account-images/avatar-account/{Guid.Parse(userToken.UserId)}/" +
                $"{Guid.Parse(userToken.UserId)}-{0}{Path.GetExtension(request.AvatarImages[0].FileName)}";

            await fileManager.Upload(
                _config.GetValue<string>("fileManagementAccessKey:publicBucket"),
                $"account-images/avatar-account/{Guid.Parse(userToken.UserId)}", avatarImg);
        }

        // saveFile
        var account = await ((IUserRepository)vpsRepository).UpdateAccountById(request);
        if (account == null) throw new ClientException();

        return Ok(new
        {
            AccessToken = JwtTokenExtension.WriteTokenByAccount(account)
        });
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetAccountProfile)]
    public async Task<IActionResult> GetAccountProfile()
    {
        var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
        var userToken = JwtTokenExtension.ReadToken(accessToken)!;
        var account =
            await ((IUserRepository)vpsRepository).GetAccountByIdAsync(
                Guid.Parse(userToken.UserId));
        if (account == null) throw new ClientException();
        var commune = account.Commune;
        var district = commune?.District;
        var city = district?.City;
        return Ok(new GetAccountProfileResponse()
        {
            FirstName = account.FirstName,
            LastName = account.LastName,
            Email = account.Email,
            Phone = account.PhoneNumber,
            City = city?.Id,
            District = district?.Id,
            Commune = commune?.Id,
            Address = account.Address,
            Role = EnumExtension.GetEnumDescription(
                EnumExtension.CoverIntToEnum<UserRoleEnum>(account.TypeId)),
            Dob = account.ParkingZoneOwner?.Dob,
            Avatar = account.Avatar,
            AddressArray = commune == null
                ? null
                : new[] { city!.Name, district!.Name, commune.Name },
            RoleId = account.TypeId
        });
    }

    [HttpPost]
    public async Task<IActionResult> AttendanceLogin(LoginRequest request)
    {
        var account =
            await ((IUserRepository)vpsRepository).GetAccountByUserNameAsync(request.Username);
        if (account == null)
        {
            throw new ClientException(5001);
        }

        if (!BCrypt.Net.BCrypt.EnhancedVerify(request.Password, account.Password))
        {
            throw new ClientException(5006);
        }

        if (account.IsVerified == false)
        {
            throw new ClientException(5007);
        }

        if (account.IsBlock)
        {
            throw new ClientException(5002);
        }

        if (account.TypeId != (int)UserRoleEnum.ATTENDANT)
        {
            throw new ClientException(5001);
        }

        var userToken = new UserTokenHeader
        {
            UserId = account.Id.ToString(),
            Email = account.Email,
            FirstName = account.FirstName,
            LastName = account.LastName,
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

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.BlockUserAccount)]
    public async Task<IActionResult> BlockUserAccount([FromBody] BlockUserAccountInput input)
    {
        var account =
            ((IUserRepository)vpsRepository).GetAccountToBlockById((Guid)input.AccountId!);
        if (account is null)
        {
            throw new ServerException(2);
        }

        if (input is { IsBlock: true, BlockReason: null })
        {
            throw new ServerException("Không thể khóa tài khoản mà không có lý do!");
        }

        account.IsBlock = (bool)input.IsBlock!;
        account.BlockReason = input.BlockReason;

        await ((IUserRepository)vpsRepository).Update(account);
        await ((IUserRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }
}