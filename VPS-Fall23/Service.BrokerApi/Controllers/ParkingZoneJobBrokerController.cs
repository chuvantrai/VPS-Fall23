using Microsoft.AspNetCore.Mvc;
using Service.BrokerApi.Models.ParkingZoneJob;
using Service.BrokerApi.Services;

namespace Service.BrokerApi.Controllers
{
    public class ParkingZoneJobBrokerController : ApiBrokerController<IRabbitMQClient>
    {
        public ParkingZoneJobBrokerController(IRabbitMQClient rabbitMQClient) : base(rabbitMQClient)
        {
        }
        [HttpPost("auto-delete-parking-zone")]
        public async Task<IActionResult> AutoDeleteParkingZone(AutoDeleteDto autoDeleteDto)
        {
            await this.rabbitMQClient.SendMessageAsync(autoDeleteDto);
            return NoContent();
        }
    }
}
