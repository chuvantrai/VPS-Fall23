using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class AttendantController : VpsController<ParkingZoneAttendant>
{
    private readonly IUserRepository _userRepository;
    private readonly IParkingZoneRepository _parkingZoneRepository;

    public AttendantController(IAttendantRepository attendantRepository,
        IUserRepository userRepository, IParkingZoneRepository parkingZoneRepository)
        : base(attendantRepository)
    {
        _userRepository = userRepository;
        _parkingZoneRepository = parkingZoneRepository;
    }

    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.CreateAttendantAccount)]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAttendantInput input)
    {
        var existedAttendantAccount =
            await _userRepository.GetAccountByUserNameAsync(input.UserName);
        if (existedAttendantAccount is not null)
        {
            throw new ServerException(2004);
        }

        var existedParkingZone =
            _parkingZoneRepository.GetParkingZoneById((Guid)input.ParkingZoneId!);
        if (existedParkingZone is null)
        {
            throw new ServerException(2005);
        }

        if (existedParkingZone.IsApprove is false or null)
        {
            throw new ServerException(2006);
        }

        var newAcc = new Account
        {
            TypeId = (int)UserRoleEnum.ATTENDANT,
            Id = Guid.NewGuid(),
            Email = $"{input.UserName}@vps.com",
            Username = input.UserName,
            Password = BCrypt.Net.BCrypt.EnhancedHashPassword(input.Password, 13),
            FirstName = input.FirstName,
            LastName = input.LastName,
            PhoneNumber = input.PhoneNumber,
            Address = input.Address,
            CommuneId = input.CommuneId,
            IsBlock = false,
            IsVerified = true,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now
        };
        var createdAccountResult = await _userRepository.Create(newAcc);
        if (createdAccountResult is null)
        {
            throw new ServerException(ResponseNotification.ADD_ERROR);
        }

        var newParkingZoneAttendant = new ParkingZoneAttendant
        {
            Id = newAcc.Id,
            ParkingZoneId = (Guid)input.ParkingZoneId!,
            CreatedAt = DateTime.Now
        };
        var createdParkingZoneAttendantResult =
            await ((IAttendantRepository)vpsRepository).Create(newParkingZoneAttendant);
        if (createdParkingZoneAttendantResult is null)
        {
            throw new ServerException(ResponseNotification.ADD_ERROR);
        }

        await ((IAttendantRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.ADD_SUCCESS);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetListAttendant)]
    public IActionResult GetListAttendant(Guid? ownerId, [FromQuery] QueryStringParameters parameters)
    {
        if (ownerId is null)
        {
            throw new ServerException("ownerId cannot be null!");
        }

        var attendantAccounts = _userRepository.GetListAttendantAccount((Guid)ownerId, parameters);
        var result = attendantAccounts.Select((x, ind) => new
        {
            Key = ind + 1,
            x.Id,
            x.Username,
            FullName = x.FirstName + " " + x.LastName,
            x.Address,
            x.PhoneNumber,
            ParkingZone = x.ParkingZoneAttendant!.ParkingZone.Name,
            x.IsBlock
        }).ToList();
        var metadata = new
        {
            attendantAccounts.TotalCount,
            attendantAccounts.PageSize,
            attendantAccounts.CurrentPage,
            attendantAccounts.TotalPages,
            attendantAccounts.HasNext,
            attendantAccounts.HasPrev,
            Data = result
        };
        return Ok(metadata);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.SearchAttendantByName)]
    public IActionResult SearchAttendantByName(Guid? ownerId, string attendantName,
        [FromQuery] QueryStringParameters parameters)
    {
        if (ownerId is null)
        {
            throw new ServerException("ownerId cannot be null!");
        }

        var attendantAccounts =
            _userRepository.SearchAttendantByName((Guid)ownerId, attendantName, parameters);
        var result = attendantAccounts.Select((x, ind) => new
        {
            Key = ind + 1,
            x.Id,
            x.Username,
            FullName = x.FirstName + " " + x.LastName,
            x.Address,
            x.PhoneNumber,
            ParkingZone = x.ParkingZoneAttendant!.ParkingZone.Name,
            x.IsBlock
        }).ToList();
        var metadata = new
        {
            attendantAccounts.TotalCount,
            attendantAccounts.PageSize,
            attendantAccounts.CurrentPage,
            attendantAccounts.TotalPages,
            attendantAccounts.HasNext,
            attendantAccounts.HasPrev,
            Data = result
        };
        return Ok(metadata);
    }
}