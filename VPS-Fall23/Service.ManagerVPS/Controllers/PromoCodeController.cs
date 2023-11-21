using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class PromoCodeController : VpsController<PromoCode>
{
    private readonly IPromoCodeParkingZoneRepository _promoCodeParkingZoneRepository;
    private readonly IParkingTransactionRepository _transactionRepository;
    private readonly IConfiguration _configuration;

    public PromoCodeController(IPromoCodeRepository promoCodeRepository,
        IPromoCodeParkingZoneRepository promoCodeParkingZoneRepository,
        IParkingTransactionRepository transactionRepository, IConfiguration configuration)
        : base(promoCodeRepository)
    {
        _promoCodeParkingZoneRepository = promoCodeParkingZoneRepository;
        _transactionRepository = transactionRepository;
        _configuration = configuration;
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetListPromoCode)]
    public IActionResult GetListPromoCode([FromQuery] Guid? ownerId,
        [FromQuery] QueryStringParameters parameters)
    {
        var promoCodePagedLst =
            ((IPromoCodeRepository)vpsRepository).GetListPromoCodeByOwnerId(ownerId, parameters);
        var metadata = new
        {
            promoCodePagedLst.TotalCount,
            promoCodePagedLst.PageSize,
            promoCodePagedLst.CurrentPage,
            promoCodePagedLst.TotalPages,
            promoCodePagedLst.HasNext,
            promoCodePagedLst.HasPrev,
            Data = promoCodePagedLst
        };
        return Ok(metadata);
    }

    [HttpPost]
    [FilterPermission(Action = ActionFilterEnum.CreateNewPromoCode)]
    public async Task<IActionResult> CreateNewPromoCode([FromBody] NewPromoCodeInput input)
    {
        var newPromoCode = new PromoCode
        {
            Id = Guid.NewGuid(),
            Code = input.Code,
            FromDate = (DateTime)input.FromDate!,
            ToDate = (DateTime)input.ToDate!,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            OwnerId = (Guid)input.OwnerId!,
            Discount = input.Discount
        };
        await ((IPromoCodeRepository)vpsRepository).Create(newPromoCode);

        var lstPromoCodeParkingZone = input.ParkingZoneIds
            .Select(parkingZoneId =>
                new PromoCodeParkingZone
                {
                    PromoCodeId = newPromoCode.Id,
                    ParkingZoneId = parkingZoneId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                })
            .ToList();
        await _promoCodeParkingZoneRepository.CreateRange(lstPromoCodeParkingZone);

        var transactions = _transactionRepository.GetParkingTransactions();
        var usersEmail = new List<string>();
        foreach (var parkingZoneId in input.ParkingZoneIds)
        {
            var emailLst = transactions
                .Where(x => x.ParkingZoneId.Equals(parkingZoneId))
                .Select(x => x.Email)
                .Distinct()
                .ToList();
            usersEmail.AddRange(emailLst);
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Constants", "FileHtml",
            "templatePromoCode.html");
        var templateString = await System.IO.File.ReadAllTextAsync(filePath);
        templateString = templateString
            .Replace("@{PROMO_CODE}", newPromoCode.Code)
            .Replace("@{FROM_DATE}", $"{newPromoCode.FromDate: dd-MM-yyyy}")
            .Replace("@{TO_DATE}", $"{newPromoCode.ToDate: dd-MM-yyyy}")
            .Replace("@{DISCOUNT}", newPromoCode.Discount.ToString());
        var brokerApiClient =
            new BrokerApiClient(_configuration.GetValue<string>("brokerApiBaseUrl"));
        await brokerApiClient.SendMail(usersEmail.ToArray(), "[VPS]Ưu đãi", templateString);

        await ((IPromoCodeRepository)vpsRepository).SaveChange();
        return Ok(ResponseNotification.ADD_SUCCESS);
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetPromoCodeDetail)]
    public IActionResult GetPromoCodeDetail([FromQuery] Guid? promoCodeId)
    {
        if (promoCodeId is null)
        {
            throw new ServerException(2);
        }

        var promoCode =
            ((IPromoCodeRepository)vpsRepository).GetPromoCodeDetailById((Guid)promoCodeId)
            ?? throw new ServerException(2);

        return Ok(new
        {
            promoCode.Id,
            promoCode.Code,
            promoCode.FromDate,
            promoCode.ToDate,
            promoCode.Discount,
            ParkingZoneLst = promoCode.PromoCodeParkingZones
                .Select(x => new
                {
                    x.ParkingZoneId,
                    x.ParkingZone.Name
                })
                .ToList()
        });
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.UpdatePromoCode)]
    public async Task<IActionResult> UpdatePromoCode([FromBody] UpdatePromoCodeInput input)
    {
        var promoCode =
            ((IPromoCodeRepository)vpsRepository).GetPromoCodeById((Guid)input.PromoCodeId!)
            ?? throw new ServerException(2);

        promoCode.Code = input.PromoCode;
        promoCode.Discount = input.Discount;
        promoCode.FromDate = (DateTime)input.FromDate!;
        promoCode.ToDate = (DateTime)input.ToDate!;
        promoCode.ModifiedAt = DateTime.Now;
        await ((IPromoCodeRepository)vpsRepository).Update(promoCode);

        await _promoCodeParkingZoneRepository.DeleteRange(promoCode.PromoCodeParkingZones.ToList());
        var lstPromoCodeParkingZone = input.ParkingZoneIds
            .Select(parkingZoneId =>
                new PromoCodeParkingZone
                {
                    PromoCodeId = promoCode.Id,
                    ParkingZoneId = parkingZoneId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                })
            .ToList();
        await _promoCodeParkingZoneRepository.CreateRange(lstPromoCodeParkingZone);

        var transactions = _transactionRepository.GetParkingTransactions();
        var usersEmail = new List<string>();
        foreach (var parkingZoneId in input.ParkingZoneIds)
        {
            var emailLst = transactions
                .Where(x => x.ParkingZoneId.Equals(parkingZoneId))
                .Select(x => x.Email)
                .Distinct()
                .ToList();
            usersEmail.AddRange(emailLst);
        }

        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Constants", "FileHtml",
            "templatePromoCode.html");
        var templateString = await System.IO.File.ReadAllTextAsync(filePath);
        templateString = templateString
            .Replace("@{PROMO_CODE}", input.PromoCode)
            .Replace("@{FROM_DATE}", $"{input.FromDate: dd-MM-yyyy}")
            .Replace("@{TO_DATE}", $"{input.ToDate: dd-MM-yyyy}")
            .Replace("@{DISCOUNT}", input.Discount.ToString());
        var brokerApiClient =
            new BrokerApiClient(_configuration.GetValue<string>("brokerApiBaseUrl"));
        await brokerApiClient.SendMail(usersEmail.ToArray(), "[VPS] Cập nhật ưu đãi",
            templateString);

        await ((IPromoCodeRepository)vpsRepository).SaveChange();
        return Ok(promoCode);
    }
}