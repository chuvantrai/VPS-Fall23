using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;
using Service.WorkerVPS.Models.ParkingZoneJob;

namespace Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices
{
    internal class AutoDeleteParkingZoneBackgroundService : BackgroundService
    {

        public AutoDeleteParkingZoneBackgroundService(
            ILogger<AutoDeleteParkingZoneBackgroundService> logger,
            QuartzServices quartzServices,
            AutoDeleteParkingZoneDequeue parkingZoneJobBroker)
            : base(logger, quartzServices, parkingZoneJobBroker)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return base.rabbitMQClient.ExecuteAsync(stoppingToken);

        }
    }
}
