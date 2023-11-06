using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers;

namespace Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices
{
    internal class AutoDeleteParkingZoneBackgroundService : BackgroundService
    {

        public AutoDeleteParkingZoneBackgroundService(
            ILogger<AutoDeleteParkingZoneBackgroundService> logger,
            QuartzServices quartzServices,
            IRabbitMQClient rabbitMQClient)
            : base(logger, quartzServices, rabbitMQClient)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return base.rabbitMQClient.ExecuteAsync(stoppingToken);

        }
    }
}
