using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

namespace Service.ManagerVPS.Controllers
{
    public class ParkingZoneAbsentController : VpsController<ParkingZoneAbsent>
    {
        public ParkingZoneAbsentController(IParkingZoneAbsentRepository vpsRepository) : base(vpsRepository)
        {
        }
    }
}
