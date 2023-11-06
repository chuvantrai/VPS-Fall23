using Microsoft.AspNetCore.Mvc;
using Service.BrokerApi.Models.ParkingZoneJob;
using Service.BrokerApi.Services;

namespace Service.BrokerApi.Controllers
{
    public class ParkingZoneJobBrokerController : ApiBrokerController<ParkingZoneJobBroker>
    {
        public ParkingZoneJobBrokerController(ParkingZoneJobBroker rabbitMQClient) : base(rabbitMQClient)
        {
        }
        [HttpPost("auto-delete-parking-zone")]
        public IActionResult AutoDeleteParkingZone(AutoDeleteDto autoDeleteDto)
        {
            this.rabbitMQClient.ExcuteAsync(autoDeleteDto);
            return NoContent();
        }
    }
}
