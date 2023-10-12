using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.FileManagement;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class ParkingZoneController : VpsController<ParkingZone>
{
    private readonly IConfiguration _config;
    readonly FileManagementConfig fileManagementConfig;

    public ParkingZoneController(IParkingZoneRepository parkingZoneRepository,
        IConfiguration config,
        IOptions<FileManagementConfig> options)
        : base(parkingZoneRepository)
    {
        _config = config;
        this.fileManagementConfig = options.Value;
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
            PriceOverTimePerHour = input.PriceOverTimePerHour,
            Slots = input.Slots
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

        await fileManager.Upload(_config.GetValue<string>("fileManagementAccessKey:publicBucket"),
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

    [HttpPut]
    public async Task<IActionResult> ChangeParkingZoneStat([FromBody] ChangeParkingZoneStat input)
    {
        var parkingZone = ((IParkingZoneRepository)vpsRepository).GetParkingZoneById(input.Id);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }

        parkingZone.IsApprove = input.IsApprove;
        parkingZone.RejectReason = input.RejectReason;
        await ((IParkingZoneRepository)vpsRepository).Update(parkingZone);
        await ((IParkingZoneRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpGet]
    public IEnumerable<ParkingZone> GetByAddress(Guid id,
        AddressType addressType = AddressType.Commune)
    {
        switch (addressType)
        {
            case AddressType.Commune:
            {
                return ((IParkingZoneRepository)this.vpsRepository).GetByCommuneId(id);
            }
            case AddressType.District:
            {
                return ((IParkingZoneRepository)this.vpsRepository).GetByDistrictId(id);
            }
            case AddressType.City:
            {
                return ((IParkingZoneRepository)this.vpsRepository).GetByCityId(id);
            }
            default: throw new ClientException(1002);
        }
    }

    [HttpGet("{parkingZoneId}/GetImageLinks")]
    public async Task<List<string>> GetImageLinks(Guid parkingZoneId)
    {
        var parkingZone = await this.vpsRepository.Find(parkingZoneId);
        string filePrefix =
            $"{Constant.PARKING_ZONE_IMG_FOLDER}/{parkingZone.OwnerId}/{parkingZoneId}";
        FileManagementClient fileManagementClient = new FileManagementClient(
            fileManagementConfig.BaseUrl,
            fileManagementConfig.AccessKey,
            fileManagementConfig.SecretKey);

        var objectResults =
            await fileManagementClient.GetObjects(fileManagementConfig.PublicBucket, filePrefix,
                true);

        return objectResults.Select(x => GetImageLink(x.Key)).ToList();
    }

    string GetImageLink(string objectPath)
    {
        return
            $"{fileManagementConfig.EndPointServer}:{fileManagementConfig.EndPointPort.Api}/{fileManagementConfig.PublicBucket}/{objectPath}";
    }
}