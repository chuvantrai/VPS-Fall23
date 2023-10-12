using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class ParkingZoneController : VpsController<ParkingZone>
{
    private readonly IConfiguration _config;

    public ParkingZoneController(IParkingZoneRepository parkingZoneRepository,
        IConfiguration config)
        : base(parkingZoneRepository)
    {
        _config = config;
    }

    [HttpPost]
    public async Task<IActionResult> Register([FromForm] RegisterParkingZone input)
    {
        var newParkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            CommuneId = input.CommuneId,
            Name = input.Name,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            OwnerId = input.OwnerId,
            DetailAddress = input.DetailAddress,
            PricePerHour = input.PricePerHour,
            PriceOverTimePerHour = input.PriceOverTimePerHour
        };

        var fileManager =
            new FileManagementClient(_config.GetValue<string>("fileManagementAccessKey:baseUrl"),
                _config.GetValue<string>("fileManagementAccessKey:accessKey"),
                _config.GetValue<string>("fileManagementAccessKey:secretKey"));
        var parkingZoneImgs = new MultipartFormDataContent();
        for (var i = 0; i < input.ParkingZoneImages.Count; i++)
        {
            var streamContent = new StreamContent(input.ParkingZoneImages[i].OpenReadStream());
            parkingZoneImgs.Add(streamContent, FileManagementClient.MULTIPART_FORM_PARAM_NAME,
                $"{newParkingZone.Id}-{i}.{Path.GetExtension(input.ParkingZoneImages[i].FileName)}");
        }

        await fileManager.Upload("nghianv-vps-public",
            $"parking-zone-images/{input.OwnerId}/{newParkingZone.Id}", parkingZoneImgs);

        var registerParkingZoneResult =
            await ((IParkingZoneRepository)vpsRepository).Create(newParkingZone);
        if (registerParkingZoneResult is null)
        {
            throw new ServerException(ResponseNotification.ADD_ERROR);
        }

        await ((IParkingZoneRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.ADD_SUCCESS);
    }
}