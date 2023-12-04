using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Controllers.Base;
using Service.ManagerVPS.ExternalClients;
using Service.ManagerVPS.Models;
using Service.ManagerVPS.Repositories.Interfaces;

using Service.ManagerVPS.FilterPermissions;

namespace Service.ManagerVPS.Controllers
{
    public class ParkingZoneAbsentController : VpsController<ParkingZoneAbsent>
    {

        private readonly IConfiguration _config;
        public ParkingZoneAbsentController(IParkingZoneAbsentRepository vpsRepository, IConfiguration config) : base(vpsRepository)
        {
            _config = config;
        }


        [HttpGet()]
        public async Task<IEnumerable<ParkingZoneAbsent>> GetAbsents(Guid parkingZoneId, DateTime? getFrom)
        {
            return await ((IParkingZoneAbsentRepository)this.vpsRepository).GetByParkingZone(parkingZoneId, getFrom);

        }
        [HttpDelete("{absentId}")]
        [FilterPermission(Action = Constants.Enums.ActionFilterEnum.CancelAbsent)]
        public async Task<IActionResult> DeleteAbsent(Guid absentId)
        {
            var absent = await vpsRepository.Find(absentId);

            if (!absent.To.HasValue)
            {

                var brokerApiClient = new BrokerApiClient(_config.GetValue<string>("brokerApiBaseUrl"));
                await brokerApiClient.RemoveDeletingPZJob(absent.Id);
            }

            await vpsRepository.Delete(absent);
            await vpsRepository.SaveChange();
            return NoContent();
        }
    }
}
