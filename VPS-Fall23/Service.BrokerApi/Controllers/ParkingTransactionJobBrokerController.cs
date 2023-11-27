using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Service.BrokerApi.Models;
using Service.BrokerApi.Models.ParkingZoneJob;
using Service.BrokerApi.Services;

namespace Service.BrokerApi.Controllers
{
    [Route("api/parking-transaction")]
    public class ParkingTransactionJobBrokerController : ApiBrokerController<IRabbitMQClient>
    {
        public ParkingTransactionJobBrokerController(IRabbitMQClient rabbitMQClient, IOptions<RabbitMQProfile> options) : base(rabbitMQClient, options)
        {
        }
        [HttpPost("create-auto-cancel-job")]
        public async Task<IActionResult> CreateAutoCancelJob(AutoCancelBookingDto autoCancelBookingDto)
        {

            await this.rabbitMQClient.SendMessageAsync(base.rabbitMQProfile.QueueInfo.CreateCancelBookingJobQueueName, autoCancelBookingDto);
            return NoContent();
        }
        [HttpDelete("{parkingTransactionId}/remove-auto-cancel-job")]
        public async Task<IActionResult> RemoveAutoCancelJob(Guid parkingTransactionId)
        {
            await this.rabbitMQClient.SendMessageAsync(base.rabbitMQProfile.QueueInfo.RemoveCancelBookingJobQueueName, parkingTransactionId);
            return NoContent();
        }
    }
}
