using Service.BrokerApi.Models;
using System.Text;
using System.Text.Json;

namespace Service.BrokerApi.Services
{
    public class ParkingZoneJobBroker : RabbitMQClient
    {
        public ParkingZoneJobBroker(RabbitMQProfile rabbitMQProfile)
            : base(rabbitMQProfile)
        {
        }

        public override Task ExcuteAsync<AutoDeleteDto>(AutoDeleteDto message)
        {
            var objectJsonString = JsonSerializer.Serialize<AutoDeleteDto>(message);
            var objectBytes = Encoding.UTF8.GetBytes(objectJsonString);
            channel.BasicPublish(exchange: string.Empty, string.Empty, true, null, objectBytes);
            return Task.CompletedTask;
        }
    }
}
