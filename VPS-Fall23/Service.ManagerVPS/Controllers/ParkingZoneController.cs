using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.FileManagement;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.DTO.Ouput;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class ParkingZoneController : VpsController<ParkingZone>
{
    private readonly IConfiguration _config;
    private readonly FileManagementConfig _fileManagementConfig;

    public ParkingZoneController(IParkingZoneRepository parkingZoneRepository,
        IConfiguration config, IOptions<FileManagementConfig> options)
        : base(parkingZoneRepository)
    {
        _config = config;
        _fileManagementConfig = options.Value;
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

    [HttpGet]
    // [FilterPermission(Action = ActionFilterEnum.GetRequestedParkingZones)]
    public async Task<IActionResult> GetRequestedParkingZones([FromQuery] QueryStringParameters parameters)
    {
        var requestedParkingZones =
            ((IParkingZoneRepository)vpsRepository).GetRequestedParkingZones(parameters);

        var result = new List<RequestedParkingZoneItemOutput>();

        foreach (var item in requestedParkingZones)
        {
            var itemImgs = await GetImageLinks(item.OwnerId, item.Id);

            result.Add(new RequestedParkingZoneItemOutput
            {
                Id = item.Id,
                Key = item.SubId,
                CommuneId = item.CommuneId,
                Name = item.Name,
                CreatedAt = $"{item.CreatedAt:dd-MM-yyyy}",
                ModifiedAt = $"{item.ModifiedAt:dd-MM-yyyy}",
                OwnerId = item.OwnerId,
                DetailAddress = item.DetailAddress,
                PricePerHour = string.Format(new CultureInfo("vi-VN"), "{0:C}", item.PricePerHour),
                PriceOverTimePerHour =
                    string.Format(new CultureInfo("vi-VN"), "{0:C}", item.PriceOverTimePerHour),
                Slots = item.Slots,
                Lat = item.Lat,
                Lng = item.Lng,
                ParkingZoneImages = itemImgs
            });
        }

        var metadata = new
        {
            requestedParkingZones.TotalCount,
            requestedParkingZones.PageSize,
            requestedParkingZones.CurrentPage,
            requestedParkingZones.TotalPages,
            requestedParkingZones.HasNext,
            requestedParkingZones.HasPrev,
            Data = result
        };

        return Ok(metadata);
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
        parkingZone.ModifiedAt = DateTime.Now;
        await ((IParkingZoneRepository)vpsRepository).Update(parkingZone);
        await ((IParkingZoneRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpGet]
    public IEnumerable<ParkingZone> GetByAddress(Guid id,
        AddressType addressType = AddressType.Commune)
    {
        return addressType switch
        {
            AddressType.Commune => ((IParkingZoneRepository)vpsRepository).GetByCommuneId(id),
            AddressType.District => ((IParkingZoneRepository)vpsRepository).GetByDistrictId(id),
            AddressType.City => ((IParkingZoneRepository)vpsRepository).GetByCityId(id),
            _ => throw new ClientException(1002)
        };
    }

    [HttpGet("{parkingZoneId}")]
    public async Task<List<string>> GetImageLinks(Guid parkingZoneId)
    {
        var parkingZone = await vpsRepository.Find(parkingZoneId);
        var result = await GetImageLinks(parkingZone.OwnerId, parkingZoneId);
        return result;
    }

    private string GetImageLink(string objectPath)
    {
        return
            $"{_fileManagementConfig.EndPointServer}:{_fileManagementConfig.EndPointPort.Api}/{_fileManagementConfig.PublicBucket}/{objectPath}";
    }

    private async Task<List<string>> GetImageLinks(Guid parkingZoneOwnerId, Guid parkingZoneId)
    {
        var filePrefix =
            $"{Constant.PARKING_ZONE_IMG_FOLDER}/{parkingZoneOwnerId}/{parkingZoneId}";

        var fileManagementClient = new FileManagementClient(
            _fileManagementConfig.BaseUrl,
            _fileManagementConfig.AccessKey,
            _fileManagementConfig.SecretKey);

        var objectResults =
            await fileManagementClient.GetObjects(_fileManagementConfig.PublicBucket, filePrefix,
                true);

        return objectResults.Select(x => GetImageLink(x.Key)).ToList();
    }
}