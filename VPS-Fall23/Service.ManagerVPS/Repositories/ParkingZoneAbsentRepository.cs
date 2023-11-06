using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class ParkingZoneAbsentRepository : VpsRepository<ParkingZoneAbsent>,
    IParkingZoneAbsentRepository
{
    public ParkingZoneAbsentRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }
}