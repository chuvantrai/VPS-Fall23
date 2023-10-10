using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneOwnerRepository : VpsRepository<ParkingZoneOwner>, IParkingZoneOwnerRepository
{
    public ParkingZoneOwnerRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }
}