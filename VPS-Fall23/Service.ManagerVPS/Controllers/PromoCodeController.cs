using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
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
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Service.ManagerVPS.Controllers;

public class PromoCodeController : VpsController<PromoCode>
{
    private readonly IPromoCodeInfoRepository _promoCodeInfoRepository;
    private readonly IParkingTransactionRepository _transactionRepository;
    private readonly IGeneralVPS _generalVps;
    private readonly TwilioSettings _twilio;

    public PromoCodeController(IPromoCodeRepository promoCodeRepository,
        IPromoCodeInfoRepository promoCodeInfoRepository,
        IParkingTransactionRepository transactionRepository, 
        IGeneralVPS generalVps,
        IOptions<TwilioSettings> twilio)
        : base(promoCodeRepository)
    {
        _promoCodeInfoRepository = promoCodeInfoRepository;
        _transactionRepository = transactionRepository;
        _generalVps = generalVps;
        _twilio = twilio.Value;
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
        var sendPromoCodeNow = ((DateTime)input.FromDate! - DateTime.Now).TotalHours < 48;
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
        var transactionLst = _transactionRepository.GetParkingTransactions();
        var promoCodeLst = new List<PromoCode>();
        foreach (var parkingZoneId in input.ParkingZoneIds)
        {
            var promoCode = transactionLst
                .Where(x =>
                    x.ParkingZoneId.Equals(parkingZoneId) &&
                    x is { Email: not null, Phone: not null })
                .DistinctBy(x => new
                {
                    x.Email,
                    x.Phone
                })
                .Select(x => new PromoCode
                {
                    Id = Guid.NewGuid(),
                    Code = _generalVps.GenerateRandomCode(6),
                    PromoCodeInformationId = newPromoCodeInfo.Id,
                    NumberOfUses = 1,
                    UserEmail = x.Email!,
                    UserPhone = x.Phone!,
                    ParkingZoneId = parkingZoneId,
                    CreatedAt = DateTime.Now,
                    ModifiedAt = DateTime.Now,
                    UserReceivedCode = sendPromoCodeNow
                })
                .ToList();
            promoCodeLst.AddRange(promoCode);
        }

        if (promoCodeLst.Count == 0)
        {
            throw new ServerException(
                "Hiện chưa có người dùng nào sử dụng bãi đỗ xe. Không thể tạo khuyến mãi!");
        }

        await ((IPromoCodeRepository)vpsRepository).CreateRange(promoCodeLst);
        await vpsRepository.SaveChange();
        if (sendPromoCodeNow)
        {
            // send code for user
            await SendNotificationPromoCodeToUser(promoCodeLst.Select(x=>x.Id),input.ParkingZoneIds);
        }

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
            .Distinct()
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
        using (var context = new FALL23_SWP490_G14Context())
        {
            List<PromoCode> list = context.PromoCodes.Include(p => p.PromoCodeInformation)
                .Where(x => x.PromoCodeInformationId == input.PromoCodeId).ToList();
            PromoCodeInformation? info = context.PromoCodeInformations
                .Where(x => x.Id == input.PromoCodeId).FirstOrDefault();
            if (list == null)
            {
                return BadRequest("Promo code not found");
            }

            if (info.IsSent)
            {
                return BadRequest("Can't Update");
            }

            context.PromoCodes.RemoveRange(list);
            if (info != null)
            {
                info.FromDate = input.FromDate;
                info.ToDate = input.ToDate;
                info.Discount = input.Discount;
                info.IsSent = false;
            }
            else
            {
                return BadRequest("Promo Code Info not exist");
            }

            var transactionLst = _transactionRepository.GetParkingTransactions();
            var promoCodeLst = new List<PromoCode>();
            foreach (var parkingZoneId in input.ParkingZoneIds)
            {
                var promoCode = transactionLst
                    .Where(x => x.ParkingZoneId.Equals(parkingZoneId))
                    .DistinctBy(x => new
                    {
                        x.Email,
                        x.Phone
                    })
                    .Select(x => new PromoCode
                    {
                        Id = Guid.NewGuid(),
                        Code = _generalVps.GenerateRandomCode(6),
                        PromoCodeInformationId = info.Id,
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

            await ((IPromoCodeRepository)vpsRepository).CreateRange(promoCodeLst);
            await vpsRepository.SaveChange();
            context.SaveChanges();
        }


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

    [HttpPost]
    public async Task<IActionResult> JobSendNotificationPromoCodeToUser()
    {
        var listPromoCode = await ((IPromoCodeRepository)vpsRepository)
            .GetListPromoCodeNeedSendCode();
        if (listPromoCode != null)
        {
            var promoCodes = listPromoCode.ToList();
            var listParkingZoneIds = promoCodes.Select(x => x.ParkingZoneId).Distinct().ToList();
            await _promoCodeInfoRepository.UpdateIsSendPromoCode(listParkingZoneIds);
            await SendMailPromoCode(promoCodes);
        }

        return Ok();
    }

    private async Task SendNotificationPromoCodeToUser(IEnumerable<Guid> listPromoCodeId,List<Guid> parkingZoneIds)
    {
        var listPromoCode = await ((IPromoCodeRepository)vpsRepository)
            .GetListPromoCodeByListId(listPromoCodeId);
        await _promoCodeInfoRepository.UpdateIsSendPromoCode(parkingZoneIds);
        await SendMailPromoCode(listPromoCode);
    }

    public async Task SendMailPromoCode(IEnumerable<PromoCode>? listPromoCode)
    {
        if (listPromoCode == null) return;
        var promoCodes = listPromoCode.ToList();
        promoCodes = promoCodes.OrderByDescending(x => x.CreatedAt).ToList();
        foreach (var promoCode in promoCodes)
        {
            var titleEmail = $"[VPS] khuyến mãi từ bãi đỗ xe {promoCode.ParkingZone.Name} dành riêng cho bạn";
            var bodyEmail =
                $"Bạn đã nhận được mã khuyến mãi <strong>{promoCode.Code}</strong> " +
                $"giảm {promoCode.PromoCodeInformation.Discount}% " +
                $"áp dụng cho email {promoCode.UserEmail} " +
                $"<p> khi đặt chỗ tại bãi đỗ xe {promoCode.ParkingZone.Name} </p>" +
                $"<p> Địa chỉ: {promoCode.ParkingZone.DetailAddress}</p>";
            await _generalVps.SendEmailAsync(promoCode.UserEmail, titleEmail, bodyEmail);
            
            // #region SenSmsByTwilio (mỗi lần gửi mất 1,15$ tạo mới acc đc free 15,5$ -> 1 acc = 13 lần gửi)
            // if (promoCode.Id.Equals(promoCodes.FirstOrDefault()!.Id) &&
            //     !string.IsNullOrEmpty(promoCode.UserPhone) && 
            //     promoCode.UserPhone.StartsWith("0") && 
            //     promoCode.UserPhone.Length == 10)
            // {
            //     var bodySms = titleEmail + " " + bodyEmail.Replace("<p>", "")
            //         .Replace("</p>", "")
            //         .Replace("<p>", "")
            //         .Replace("<strong>", "")
            //         .Replace("</strong>", "");
            //     
            //     promoCode.UserPhone = "+84" + promoCode.UserPhone[1..];
            //     TwilioClient.Init(_twilio.AccountSid, _twilio.AuthToken);
            //     var result = await MessageResource.CreateAsync(
            //         body: bodySms,
            //         from: new Twilio.Types.PhoneNumber(_twilio.TwilioPhoneNumber),
            //         to: promoCode.UserPhone
            //     );
            // }
            // #endregion
        }
    }
}