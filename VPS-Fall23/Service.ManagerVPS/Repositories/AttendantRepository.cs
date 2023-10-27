using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Repositories;

public class AttendantRepository : VpsRepository<ParkingZoneAttendant>, IAttendantRepository
{
    public AttendantRepository(FALL23_SWP490_G14Context context) : base(context)
    {
    }
}