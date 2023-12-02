using Microsoft.EntityFrameworkCore;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class PromoCodeRepository : VpsRepository<PromoCode>, IPromoCodeRepository
{
    public PromoCodeRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }

    public async Task<PromoCode?> GetByCode(string promoCode, Guid parkingZoneId)
    {
        return await entities
            .Include(x => x.PromoCodeInformation)
            .FirstOrDefaultAsync(p =>
                promoCode.Equals(p.Code) && p.NumberOfUses > 0 &&
                p.ParkingZoneId.Equals(parkingZoneId)
            );
    }
}