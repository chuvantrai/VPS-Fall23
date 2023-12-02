using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Enums;
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
    private readonly IPromoCodeInfoRepository _promoCodeInfoRepository;
    private readonly IParkingTransactionRepository _transactionRepository;
    private readonly IGeneralVPS _generalVps;

    public PromoCodeController(IPromoCodeRepository promoCodeRepository,
        IPromoCodeInfoRepository promoCodeInfoRepository,
        IParkingTransactionRepository transactionRepository, IGeneralVPS generalVps)
        : base(promoCodeRepository)
    {
        _promoCodeInfoRepository = promoCodeInfoRepository;
        _transactionRepository = transactionRepository;
        _generalVps = generalVps;
    }

    [HttpGet]
    [FilterPermission(Action = ActionFilterEnum.GetListPromoCode)]
    public IActionResult GetListPromoCode([FromQuery] Guid? ownerId,
        [FromQuery] QueryStringParameters parameters)
    {
        if (ownerId is null)
            throw new ServerException("ownerId cannot be null!");

        var promoCodePagedLst =
            _promoCodeInfoRepository.GetListPromoCodeByOwnerId((Guid)ownerId, parameters);

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
        var transactionLst = _transactionRepository.GetParkingTransactions();
        var promoCodeLst = new List<PromoCode>();
        foreach (var parkingZoneId in input.ParkingZoneIds)
        {
            var promoCode = transactionLst
                .Where(x => x.ParkingZoneId.Equals(parkingZoneId))
                .DistinctBy(x => x.Email)
                .Select(x => new PromoCode
                {
                    Id = Guid.NewGuid(),
                    Code = _generalVps.GenerateRandomCode(6),
                    NumberOfUses = 1,
                    UserEmail = x.Email,
                    UserPhone = x.Phone,
                    ParkingZoneId = parkingZoneId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now
                })
                .ToList();
            promoCodeLst.AddRange(promoCode);
        }

        if (promoCodeLst.Count == 0)
        {
            throw new ServerException(
                "Hiện chưa có người dùng nào sử dụng bãi đỗ xe. Không thể tạo khuyến mãi!");
        }

        var newPromoCodeInfo = new PromoCodeInformation
        {
            Id = Guid.NewGuid(),
            FromDate = (DateTime)input.FromDate!,
            ToDate = (DateTime)input.ToDate!,
            CreatedAt = DateTime.Now,
            ModifiedAt = DateTime.Now,
            OwnerId = (Guid)input.OwnerId!,
            Discount = (int)input.Discount!,
            IsSent = false
        };
        await _promoCodeInfoRepository.Create(newPromoCodeInfo);

        await ((IPromoCodeRepository)vpsRepository).CreateRange(promoCodeLst);
        await vpsRepository.SaveChange();

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

        var promoCodeInfo = _promoCodeInfoRepository.GetPromoCodeInfoDetailById((Guid)promoCodeId)
                            ?? throw new ServerException(2);
        var parkingZoneIdLst = promoCodeInfo.PromoCodes
            .Select(x => x.ParkingZoneId)
            .ToList();
        return Ok(new
        {
            promoCodeInfo.Id,
            promoCodeInfo.FromDate,
            promoCodeInfo.ToDate,
            promoCodeInfo.OwnerId,
            promoCodeInfo.Discount,
            promoCodeInfo.IsSent,
            parkingZoneIdLst
        });
    }

    [HttpPut]
    [FilterPermission(Action = ActionFilterEnum.UpdatePromoCode)]
    public async Task<IActionResult> UpdatePromoCode([FromBody] UpdatePromoCodeInput input)
    {
        return Ok(ResponseNotification.UPDATE_SUCCESS);
    }

    [HttpDelete]
    [FilterPermission(Action = ActionFilterEnum.DeletePromoCode)]
    public async Task<IActionResult> DeletePromoCode([FromQuery] Guid? promoCodeId)
    {
        if (promoCodeId is null)
            throw new ServerException(2);

        var promoCodeInfo = _promoCodeInfoRepository.GetPromoCodeInfoDetailById((Guid)promoCodeId)
                            ?? throw new ServerException(2);
        if (promoCodeInfo.IsSent)
            throw new ServerException("Mã khuyến mãi đã gửi cho người dùng không thể xóa!");

        await ((IPromoCodeRepository)vpsRepository).DeleteRange(promoCodeInfo.PromoCodes.ToList());
        await _promoCodeInfoRepository.Delete(promoCodeInfo);
        await vpsRepository.SaveChange();

        return Ok(ResponseNotification.DELETE_SUCCESS);
    }

    [HttpGet("{promoCode}")]
    public async Task<PromoCode> GetPromoCode(string promoCode, Guid parkingZoneId)
    {
        var promo = await ((IPromoCodeRepository)vpsRepository).GetByCode(promoCode, parkingZoneId);
        if (promo == null) throw new ClientException(1006);
        if (promo.PromoCodeInformation.FromDate > DateTime.Now ||
            promo.PromoCodeInformation.ToDate < DateTime.Now)
        {
            throw new ClientException(1007);
        }

        return promo;
    }
}