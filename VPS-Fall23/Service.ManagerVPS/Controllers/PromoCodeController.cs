using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
using Service.ManagerVPS.Constants.KeyValue;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.DTO.Exceptions;
using Service.ManagerVPS.DTO.Input;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Extensions.ILogic;
using Service.ManagerVPS.FilterPermissions;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers;

public class PromoCodeController : VpsController<PromoCode>
{
    private readonly IPromoCodeParkingZoneRepository _promoCodeParkingZoneRepository;
    private readonly IParkingTransactionRepository _transactionRepository;
    private readonly IGeneralVPS _generalVps;

    public PromoCodeController(IPromoCodeRepository promoCodeRepository,
        IPromoCodeParkingZoneRepository promoCodeParkingZoneRepository,
        IParkingTransactionRepository transactionRepository, IGeneralVPS generalVps)
        : base(promoCodeRepository)
    {
        _promoCodeParkingZoneRepository = promoCodeParkingZoneRepository;
        _transactionRepository = transactionRepository;
        _generalVps = generalVps;
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
            OwnerId = (Guid)input.OwnerId!
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

        var keyValuesTemplate = new List<KeyValue>
        {
            new()
            {
                Key = KeyHtmlEmail.PROMO_CODE,
                Value = newPromoCode.Code
            },
            new()
            {
                Key = KeyHtmlEmail.FROM_DATE,
                Value = $"{newPromoCode.FromDate:dd/MM/yyyy}"
            },
            new()
            {
                Key = KeyHtmlEmail.TO_DATE,
                Value = $"{newPromoCode.ToDate:dd/MM/yyyy}"
            }
        };
        var templateEmail =
            _generalVps.CreateTemplateEmail(keyValuesTemplate, "templatePromoCode.html");
        var transactions = _transactionRepository.GetParkingTransactions();
        foreach (var parkingZoneId in input.ParkingZoneIds)
        {
            var emailLst = transactions
                .Where(x => x.ParkingZoneId.Equals(parkingZoneId))
                .Select(x => x.Email)
                .Distinct()
                .ToList();
            await _generalVps.SendListEmailAsync(emailLst, "[VPS]Ưu đãi", templateEmail);
        }

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
            ParkingZoneLst = promoCode.PromoCodeParkingZones
                .Select(x => new
                {
                    x.ParkingZoneId,
                    x.ParkingZone.Name
                })
                .ToList()
        });
    }
}