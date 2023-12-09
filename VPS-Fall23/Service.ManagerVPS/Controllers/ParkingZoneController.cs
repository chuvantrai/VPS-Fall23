using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.FileManagement;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.AppSetting;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.FileManagement;
using Service.ManagerVPS.DTO.GoongMap;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.DTO.Output;
using Service.ManagerVPS.Extensions.StaticLogic;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class ParkingZoneController : VpsController<ParkingZone>
{
    private readonly IConfiguration _config;
    private readonly FileManagementConfig _fileManagementConfig;
    private readonly IContractRepository _contractRepository;
    readonly IParkingTransactionRepository parkingTransactionRepository;
    readonly IUserRepository userRepository;
    private readonly IParkingZoneAbsentRepository _absentRepository;
    readonly ParkingZoneConfig parkingZoneConfig;

    public ParkingZoneController(IParkingZoneRepository parkingZoneRepository,
        IConfiguration config, IOptions<FileManagementConfig> options,
        IContractRepository contractRepository,
        IParkingTransactionRepository parkingTransactionRepository,
        IUserRepository userRepository,
        IParkingZoneAbsentRepository absentRepository,
        IOptions<ParkingZoneConfig> pzConfigOption)
        : base(parkingZoneRepository)
    {
        _config = config;
        _fileManagementConfig = options.Value;
        _contractRepository = contractRepository;
        this.parkingTransactionRepository = parkingTransactionRepository;
        this.userRepository = userRepository;
        _absentRepository = absentRepository;
        this.parkingZoneConfig = pzConfigOption.Value;
    }

    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.RegisterNewParkingZone)]
    public async Task<IActionResult> Register([FromForm] RegisterParkingZone input)
    {
        var newParkingZone = new ParkingZone
        {
            Id = Guid.NewGuid(),
            CommuneId = input.CommuneId,
            Name = input.Name,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            OwnerId = (Guid)input.OwnerId!,
            DetailAddress = input.DetailAddress,
            PricePerHour = (decimal)input.PricePerHour!,
            PriceOverTimePerHour = (decimal)input.PriceOverTimePerHour!,
            Slots = input.Slots,
            WorkFrom = (TimeSpan)input.WorkFrom!,
            WorkTo = (TimeSpan)input.WorkTo!,
            IsFull = false,
            Location = input.Location.GetTopologyPoint()
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
    [FilterPermission(Action = ActionFilterEnum.GetAllParkingZones)]
    public IActionResult GetAll([FromQuery] QueryStringParameters parameters)
    {
        try
        {
            var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
            var userToken = JwtTokenExtension.ReadToken(accessToken)!;
            var list = ((IParkingZoneRepository)vpsRepository).GetAllParkingZone(parameters);

            if (userToken.RoleId == 3) return NotFound();
            if (userToken.RoleId == 2)
            {
                Guid ownerId = new Guid(userToken.UserId);
                list = ((IParkingZoneRepository)vpsRepository).GetOwnerParkingZone(parameters,
                    ownerId);
            }


            var metadata = new
            {
                list.TotalCount,
                list.PageSize,
                list.CurrentPage,
                list.TotalPages,
                list.HasNext,
                list.HasPrev,
                Data = list
            };
            return Ok(metadata);
        }
        catch (Exception ex)
        {
            return NotFound(ex);
        }
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetAllParkingZones)]
    public IActionResult GetByName([FromQuery] QueryStringParameters parameters, string name)
    {
        try
        {
            var accessToken = Request.Cookies["ACCESS_TOKEN"]!;
            var userToken = JwtTokenExtension.ReadToken(accessToken)!;
            var list =
                ((IParkingZoneRepository)vpsRepository).GetParkingZoneByName(parameters, name);
            List<ParkingZoneItemOutput> res = new List<ParkingZoneItemOutput>();

            if (userToken.RoleId == 3) return NotFound();
            if (userToken.RoleId == 2)
            {
                Guid ownerId = new Guid(userToken.UserId);
                list = ((IParkingZoneRepository)vpsRepository).GetOwnerParkingZoneByName(parameters,
                    name, ownerId);
            }


            var metadata = new
            {
                list.TotalCount,
                list.PageSize,
                list.CurrentPage,
                list.TotalPages,
                list.HasNext,
                list.HasPrev,
                Data = list
            };
            return Ok(metadata);
        }
        catch (Exception ex)
        {
            return NotFound(ex);
        }
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetAllParkingZoneByOwnerId)]
    public IActionResult GetAllParkingZoneByOwnerId([FromQuery] string ownerId)
    {
        var parkingZoneList =
            ((IParkingZoneRepository)vpsRepository).GetParkingZoneByOwnerId(ownerId);
        var result = parkingZoneList.Select(x => new
        {
            Value = x.Id,
            Label = x.Name
        }).ToList();
        return Ok(result);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetApprovedParkingZoneByOwnerId)]
    public IActionResult GetApprovedParkingZoneByOwnerId([FromQuery] Guid ownerId)
    {
        var parkingZoneLst =
            ((IParkingZoneRepository)vpsRepository).GetApprovedParkingZonesByOwnerId(ownerId);
        var result = parkingZoneLst.Select(x => new
        {
            Value = x.Id,
            Label = x.Name
        }).ToList();
        return Ok(result);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetRequestedParkingZones)]
    public async Task<IActionResult> GetRequestedParkingZones(
        [FromQuery] QueryStringParameters parameters)
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
                /*                Lat = item.Lat,
                                Lng = item.Lng,*/
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

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetAdminOverview)]
    public async Task<IActionResult> GetAdminOverview()
    {
        var now = DateTime.Now;
        var list = await parkingTransactionRepository.GetAll();

        var customerList = list.GroupBy(x => x.Phone).Select(group => group.Key).ToList();
        var phonesWithTransactionsThisMonth = list.Where(t =>
                t.CreatedAt.Month == now.Month && t.CreatedAt.Year == now.Year)
            .Select(x => x.Phone).Distinct().ToList();
        var phonesWithFirstTimeTransactions = list.GroupBy(t => t.Phone)
            .Where(group =>
                group.Min(t => t.CreatedAt) ==
                group.Min(t => t.CreatedAt)) // Find the earliest transaction for each phone number
            .Select(group => group.Key)
            .ToList();
        var listOwner = userRepository.GetAllOwnerAccount();
        var listParkingZone = await ((IParkingZoneRepository)vpsRepository).GetAllParkingZone();

        DateTime currentDate = DateTime.Now;
        DayOfWeek startOfWeek = DayOfWeek.Monday; // Define the start day of the week

        DateTime startDate = currentDate.Date;
        while (startDate.DayOfWeek != startOfWeek)
        {
            startDate = startDate.AddDays(-1);
        }

        DateTime endDate = startDate.AddDays(7).AddSeconds(-1);

        var done = 0;
        var notCheckIn = 0;
        var notCheckout = 0;
        foreach (var item in list)
        {
            if (item.CheckinBy != null && item.CheckoutBy != null)
            {
                done++;
            }

            if (item.CheckinBy != null && item.CheckoutBy == null)
            {
                notCheckout++;
            }
        }

        var customerData = new
        {
            totalCustomer = list.Count,
            oneHours = list.Where(x =>
                x.CreatedAt.Hour == now.Hour && x.CreatedAt.Day == now.Day &&
                x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).ToList().Count,
            oneDay = list.Where(x =>
                x.CreatedAt.Day == now.Day && x.CreatedAt.Month == now.Month &&
                x.CreatedAt.Year == now.Year).ToList().Count,
            oneWeek = list.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate).ToList()
                .Count,
            oneMonth =
                list.Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year)
                    .ToList().Count,
            oneYear = list.Where(x => x.CreatedAt.Year == now.Year).ToList().Count,
        };
        var ownerData = new
        {
            totalOwner = listOwner.Count,
            oneHours = listOwner.Where(x =>
                x.CreatedAt.Hour == now.Hour && x.CreatedAt.Day == now.Day &&
                x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).ToList().Count,
            oneDay = listOwner.Where(x =>
                x.CreatedAt.Day == now.Day && x.CreatedAt.Month == now.Month &&
                x.CreatedAt.Year == now.Year).ToList().Count,
            oneWeek = listOwner.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .ToList().Count,
            oneMonth = listOwner
                .Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).ToList()
                .Count,
            oneYear = listOwner.Where(x => x.CreatedAt.Year == now.Year).ToList().Count,
        };
        var parkingZoneData = new
        {
            totalParkingZone = listParkingZone.Count,
            oneHours = listParkingZone.Where(x =>
                x.CreatedAt.Hour == now.Hour && x.CreatedAt.Day == now.Day &&
                x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).ToList().Count,
            oneDay = listParkingZone.Where(x =>
                x.CreatedAt.Day == now.Day && x.CreatedAt.Month == now.Month &&
                x.CreatedAt.Year == now.Year).ToList().Count,
            oneWeek = listParkingZone.Where(x => x.CreatedAt >= startDate && x.CreatedAt <= endDate)
                .ToList().Count,
            oneMonth = listParkingZone
                .Where(x => x.CreatedAt.Month == now.Month && x.CreatedAt.Year == now.Year).ToList()
                .Count,
            oneYear = listParkingZone.Where(x => x.CreatedAt.Year == now.Year).ToList().Count,
        };

        var data = new
        {
            customerTotal = customerList.Count,
            customerData = customerData,
            activeCustomer = phonesWithTransactionsThisMonth.Count,
            newCustomer = phonesWithFirstTimeTransactions.Count,
            ownerData = ownerData,
            parkingZoneData = parkingZoneData,
            done = done,
            notCheckIn = list.Count() - notCheckout - done,
            notCheckout = notCheckout,
        };

        return Ok(data);
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.ChangeParkingZoneStat)]
    public async Task<IActionResult> ChangeParkingZoneStat([FromBody] ChangeParkingZoneStat input)
    {
        var output =
            ((IParkingZoneRepository)vpsRepository).GetParkingZoneAndOwnerByParkingZoneId(
                (Guid)input.Id!);
        if (output is null)
        {
            throw new ServerException(2);
        }

        var parkingZone = output.ParkingZone;
        parkingZone.IsApprove = input.IsApprove;
        parkingZone.RejectReason = input.RejectReason;
        parkingZone.ModifiedAt = DateTime.Now;
        await ((IParkingZoneRepository)vpsRepository).Update(parkingZone);

        await ((IParkingZoneRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.ChangeParkingZoneFullStatus)]
    public async Task<IActionResult> ChangeParkingZoneFullStatus(
        [FromBody] ChangeParkingZoneFullStatus input)
    {
        var parkingZone =
            ((IParkingZoneRepository)vpsRepository).GetParkingZoneById((Guid)input.ParkingZoneId!);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }

        parkingZone.IsFull = input.IsFull;
        parkingZone.ModifiedAt = DateTime.Now;
        await ((IParkingZoneRepository)vpsRepository).Update(parkingZone);
        await ((IParkingZoneRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.CloseParkingZone)]
    public async Task<IActionResult> CloseParkingZone([FromBody] CloseParkingZoneInput input)
    {
        var parkingZone =
            ((IParkingZoneRepository)vpsRepository).GetParkingZoneAndAbsentById(
                (Guid)input.ParkingZoneId!);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }

        if (parkingZone.IsApprove is null or false)
        {
            throw new ServerException(
                "Bãi đỗ xe đang chờ duyệt hoặc bị từ chối không thể đóng cửa!");
        }

        var newAbsent = new ParkingZoneAbsent
        {
            Id = Guid.NewGuid(),
            ParkingZoneId = (Guid)input.ParkingZoneId,
            From = (DateTime)input.CloseFrom!,
            To = input.CloseTo,
            Reason = input.Reason,
            CreatedAt = DateTime.Now
        };
        if (input.CloseTo is null)
        {
            var dayDelete = _config.GetValue<int>("parkingZone:removeAfterCloseInDay");
            var brokerApiClient = new BrokerApiClient(
                _config.GetValue<string>("brokerApiBaseUrl")
            );
            await brokerApiClient.CreateDeletingPZJob(newAbsent.Id, newAbsent.ParkingZoneId,
                newAbsent.From.AddDays(dayDelete));
        }

        await _absentRepository.Create(newAbsent);
        await _absentRepository.SaveChange();

        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetParkingZoneInfoById)]
    public async Task<IActionResult> GetParkingZoneInfoById(Guid parkingZoneId)
    {
        var parkingZone = ((IParkingZoneRepository)vpsRepository).GetParkingZoneById(parkingZoneId);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }

        var parkingZoneImages = await GetImageLinks(parkingZone.OwnerId, parkingZone.Id);

        var result = new
        {
            parkingZone.Id,
            Key = parkingZone.SubId,
            Commune = parkingZone.Commune?.Name,
            District = parkingZone.Commune?.District.Name,
            City = parkingZone.Commune?.District.City.Name,
            parkingZone.Name,
            CreatedAt = $"{parkingZone.CreatedAt:dd-MM-yyyy}",
            ModifiedAt = $"{parkingZone.ModifiedAt:dd-MM-yyyy}",
            parkingZone.OwnerId,
            parkingZone.DetailAddress,
            parkingZone.PricePerHour,
            parkingZone.PriceOverTimePerHour,
            parkingZone.Slots,
            parkingZone.WorkFrom,
            parkingZone.WorkTo,
            parkingZone.IsFull,
            parkingZone.ParkingZoneAbsents,
            ParkingZoneImages = parkingZoneImages
        };
        return Ok(result);
    }

    [HttpPatch]
    [FilterPermission(Action = ActionFilterEnum.UpdateParkingZone)]
    public async Task<ParkingZone> UpdateParkingZoneAddress(
        [FromBody] UpdateParkingZoneAddressInput updateParkingZoneAddressInput)
    {
        var parkingZone =
            ((IParkingZoneRepository)vpsRepository).GetParkingZoneById(updateParkingZoneAddressInput
                .ParkingZoneId) ?? throw new ServerException(2);
        if (parkingZone.IsApprove == true)
        {
            throw new ServerException("Bãi đỗ xe đã xác thực. Không thể thay đổi thông tin!");
        }

        parkingZone.Location = updateParkingZoneAddressInput.Location.GetTopologyPoint();
        parkingZone.DetailAddress = updateParkingZoneAddressInput.DetailAddress;
        parkingZone.IsApprove = null;
        await ((IParkingZoneRepository)vpsRepository).Update(parkingZone);
        await ((IParkingZoneRepository)vpsRepository).SaveChange();
        return parkingZone;
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.UpdateParkingZone)]
    public async Task<IActionResult> UpdateParkingZone([FromForm] UpdateParkingZoneInput input)
    {
        var parkingZone =
            ((IParkingZoneRepository)vpsRepository).GetParkingZoneById((Guid)input.ParkingZoneId!);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }

        if (parkingZone.IsApprove == true)
        {
            throw new ServerException("Bãi đỗ xe đã xác thực. Không thể thay đổi thông tin!");
        }

        parkingZone.Name = input.ParkingZoneName;
        parkingZone.PricePerHour = (decimal)input.PricePerHour!;
        parkingZone.PriceOverTimePerHour = (decimal)input.PriceOverTimePerHour!;
        parkingZone.Slots = (int)input.Slots!;
        parkingZone.WorkFrom = input.WorkFrom;
        parkingZone.WorkTo = input.WorkTo;
        parkingZone.IsApprove = null;
        parkingZone.ModifiedAt = DateTime.Now;

        var parkingZoneImages = await GetImageLinks(parkingZone.OwnerId, parkingZone.Id);
        var fileManager =
            new FileManagementClient(_config.GetValue<string>("fileManagementAccessKey:baseUrl"),
                _config.GetValue<string>("fileManagementAccessKey:accessKey"),
                _config.GetValue<string>("fileManagementAccessKey:secretKey"));

        // delete old images
        if (parkingZoneImages.Count > 0)
        {
            var removeObjectsDtos = parkingZoneImages
                .Select(img => new RemoveObjectsDto
                {
                    ObjectName =
                        img.Replace(
                            $"{_fileManagementConfig.EndPointServer}:{_fileManagementConfig.EndPointPort.Api}/{_fileManagementConfig.PublicBucket}/",
                            "")
                })
                .ToList();
            await fileManager.RemoveMultipleObjects(
                _config.GetValue<string>("fileManagementAccessKey:publicBucket"),
                removeObjectsDtos);
        }

        // add new images
        var parkingZoneImgs = new MultipartFormDataContent();
        for (var i = 0; i < input.ParkingZoneImages.Count; i++)
        {
            var streamContent = new StreamContent(input.ParkingZoneImages[i].OpenReadStream());
            parkingZoneImgs.Add(streamContent, FileManagementClient.MULTIPART_FORM_PARAM_NAME,
                $"{parkingZone.Id}-{i}.{Path.GetExtension(input.ParkingZoneImages[i].FileName)}");
        }

        await fileManager.Upload(_config.GetValue<string>("fileManagementAccessKey:publicBucket"),
            $"parking-zone-images/{parkingZone.OwnerId}/{parkingZone.Id}", parkingZoneImgs);

        await ((IParkingZoneRepository)vpsRepository).Update(parkingZone);
        await ((IParkingZoneRepository)vpsRepository).SaveChange();

        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpDelete("{parkingZoneId:guid}")]
    // [FilterPermission(Action = ActionFilterEnum.DeleteParkingZoneAndAbsent)]
    public async Task<IActionResult> DeleteParkingZone(Guid parkingZoneId)
    {
        var parkingZone = await ((IParkingZoneRepository)vpsRepository).Find(parkingZoneId);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }

        if (parkingZone.ParkingZoneAbsents is null)
        {
            throw new ServerException("Bãi đỗ xe chưa đóng cửa!");
        }

        await vpsRepository.Delete(parkingZone);
        await vpsRepository.SaveChange();
        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpDelete]
    [FilterPermission(Action = ActionFilterEnum.DeleteParkingZoneAction)]
    public async Task<IActionResult> DeleteParkingZoneAction([FromQuery] Guid? parkingZoneId)
    {
        if (parkingZoneId is null)
        {
            throw new ServerException("Parking Zone Id cannot be null!");
        }

        var parkingZone = await ((IParkingZoneRepository)vpsRepository).Find(parkingZoneId);
        if (parkingZone is null)
        {
            throw new ServerException(2);
        }

        if (parkingZone.IsApprove is true)
        {
            throw new ServerException("Không thể xóa bãi đỗ xe đã xác thực!");
        }

        var parkingZoneImages = await GetImageLinks(parkingZone.OwnerId, parkingZone.Id);
        var fileManager =
            new FileManagementClient(_config.GetValue<string>("fileManagementAccessKey:baseUrl"),
                _config.GetValue<string>("fileManagementAccessKey:accessKey"),
                _config.GetValue<string>("fileManagementAccessKey:secretKey"));

        // delete old images
        if (parkingZoneImages.Count > 0)
        {
            var removeObjectsDtos = parkingZoneImages
                .Select(img => new RemoveObjectsDto
                {
                    ObjectName =
                        img.Replace(
                            $"{_fileManagementConfig.EndPointServer}:{_fileManagementConfig.EndPointPort.Api}/{_fileManagementConfig.PublicBucket}/",
                            "")
                })
                .ToList();
            await fileManager.RemoveMultipleObjects(
                _config.GetValue<string>("fileManagementAccessKey:publicBucket"),
                removeObjectsDtos);
        }

        await vpsRepository.Delete(parkingZone);
        await vpsRepository.SaveChange();

        return Ok(ResponseNotification.DELETE_SUCCESS);
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

    [HttpGet]
    public IEnumerable<ParkingZone> GetNearAround([FromQuery] Position position)
    {
        return ((IParkingZoneRepository)vpsRepository).GetParkingZoneNearAround(position,
            parkingZoneConfig.RadiusFindNearAround);
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

    [HttpGet("{parkingZoneId}")]
    public async Task<int> GetBookedSlot(Guid parkingZoneId, DateTime? checkAt = null)
    {
        return await parkingTransactionRepository.GetBookedSlot(parkingZoneId,
            checkAt ?? DateTime.Now);
    }

    [HttpPost]
    public IEnumerable<ParkingZone>? GetDataParkingZoneByParkingZoneIds(Guid[]? parkingZoneIds)
    {
        return ((IParkingZoneRepository)vpsRepository).GetParkingZoneByArrayParkingZoneId(
            parkingZoneIds);
    }

    [HttpGet("{attendantId}")]
    public string GetFreeSlotByAttendantId(Guid attendantId)
    {
        return ((IParkingZoneRepository)vpsRepository).GetFreeSlotByAttendantId(attendantId);
    }
}