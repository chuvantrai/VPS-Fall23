using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class PromoCodeParkingZoneRepository : VpsRepository<PromoCodeParkingZone>,
    IPromoCodeParkingZoneRepository
{
    public PromoCodeParkingZoneRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }
}