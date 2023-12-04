using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class PromoCodeRepository : VpsRepository<PromoCode>, IPromoCodeRepository
{
    public PromoCodeRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public PagedList<PromoCode> GetListPromoCodeByOwnerId(Guid ownerId,
        QueryStringParameters parameters)
    {
        var promoCodeLst = entities.Where(x => x.OwnerId.Equals(ownerId));
        return PagedList<PromoCode>.ToPagedList(promoCodeLst, parameters.PageNumber,
            parameters.PageSize);
    }

    public PromoCode? GetPromoCodeDetailById(Guid id)
    {
        var promoCode = entities
            .Include(x => x.PromoCodeParkingZones)
            .ThenInclude(x => x.ParkingZone)
            .FirstOrDefault(x => x.Id.Equals(id));
        return promoCode;
    }

    public PromoCode? GetPromoCodeById(Guid id)
    {
        var promoCode = entities
            .Include(x => x.PromoCodeParkingZones)
            .FirstOrDefault(x => x.Id.Equals(id));
        return promoCode;
    }
    public async Task<PromoCode?> GetByCode(string promoCode, Guid parkingZoneId)
    {
        return await entities.FirstOrDefaultAsync(p => promoCode.Equals(p.Code) && p.NumberOfUses > 0 && p.PromoCodeParkingZones.Any(pcpz => pcpz.ParkingZoneId == parkingZoneId));
    }
}