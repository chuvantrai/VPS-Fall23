using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.BrokerApi.Models;
using Service.BrokerApi.Models.ParkingZoneJob;
using Service.BrokerApi.Services;

namespace Service.BrokerApi.Controllers
{
    [Route("api/parking-zone")]
    public class ParkingZoneJobBrokerController : ApiBrokerController<IRabbitMQClient>
    {
        public ParkingZoneJobBrokerController(IRabbitMQClient rabbitMQClient, IOptions<RabbitMQProfile> options)
            : base(rabbitMQClient, options)
        {
        }
        [HttpPost("create-deleting-parking-zone-job")]
        public async Task<IActionResult> AutoDeleteParkingZone(AutoDeleteDto autoDeleteDto)
        {
            await this.rabbitMQClient.SendMessageAsync(base.rabbitMQProfile.QueueInfo.CreateDeletingPZJobQueueName, autoDeleteDto);
            return NoContent();
        }
        [HttpDelete("{parkingZoneAbsentId}/remove-deleting-parking-zone-job")]
        public async Task<IActionResult> RemoveDeletingParkingZoneJob(Guid parkingZoneAbsentId)
        {
            await this.rabbitMQClient.SendMessageAsync(base.rabbitMQProfile.QueueInfo.RemoveDeletingPZJobQueueName, parkingZoneAbsentId);
            return NoContent();
        }
    }
}
