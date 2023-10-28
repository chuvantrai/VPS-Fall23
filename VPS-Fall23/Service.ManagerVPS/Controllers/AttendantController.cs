using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
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
}