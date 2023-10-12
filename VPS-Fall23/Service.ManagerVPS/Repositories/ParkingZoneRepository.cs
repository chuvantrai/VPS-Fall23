using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneRepository : VpsRepository<ParkingZone>, IParkingZoneRepository
{
    public ParkingZoneRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }
}