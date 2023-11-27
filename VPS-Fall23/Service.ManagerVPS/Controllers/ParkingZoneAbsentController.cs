using Microsoft.AspNetCore.Mvc;
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


        [HttpGet()]
        public async Task<IEnumerable<ParkingZoneAbsent>> GetAbsents(Guid parkingZoneId, DateTime? getFrom)
        {
            return await ((IParkingZoneAbsentRepository)this.vpsRepository).GetByParkingZone(parkingZoneId, getFrom);

        }
    }
}
