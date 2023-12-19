using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.DTO.OtherModels;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class PromoCodeInfoRepository : VpsRepository<PromoCodeInformation>,
    IPromoCodeInfoRepository
{
    private FALL23_SWP490_G14Context _context { get; set; }
    public PromoCodeInfoRepository(FALL23_SWP490_G14Context context) : base(context)
    {
        _context = context;
    }

    public PagedList<PromoCodeInformation> GetListPromoCodeByOwnerId(Guid ownerId,
        QueryStringParameters parameters)
    {
        var lstPromoCodeInfo = entities
            .Include(x => x.PromoCodes)
            .ThenInclude(x => x.ParkingZone)
            .Where(x => x.OwnerId.Equals(ownerId));
        return PagedList<PromoCodeInformation>.ToPagedList(lstPromoCodeInfo, parameters.PageNumber,
            parameters.PageSize);
    }

    public PromoCodeInformation? GetPromoCodeInfoDetailById(Guid infoId)
    {
        var promoCodeInfo = entities
            .Include(x => x.PromoCodes)
            .FirstOrDefault(x => x.Id.Equals(infoId));
        return promoCodeInfo;
    }

    public async Task UpdateIsSendPromoCode(List<Guid> parkingZoneIds)
    {
        var listPromoCodeInfo = 
            entities.Include(x => x.PromoCodes)
            .Where(x => parkingZoneIds.Contains(x.PromoCodes.First().ParkingZoneId));
        foreach (var info in listPromoCodeInfo)
        {
            info.IsSent = true;
        }
        await _context.SaveChangesAsync();
    }
}