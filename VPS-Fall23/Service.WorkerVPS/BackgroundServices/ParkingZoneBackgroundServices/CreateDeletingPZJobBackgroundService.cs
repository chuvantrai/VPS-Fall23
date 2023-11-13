using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;

namespace Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices
{
    internal class CreateDeletingPZJobBackgroundService : BackgroundService
    {

        public CreateDeletingPZJobBackgroundService(
            ILogger<CreateDeletingPZJobBackgroundService> logger,
            QuartzServices quartzServices,
            CreateDeletingPZJobDequeue rabbitMQClient)
            : base(logger, quartzServices, rabbitMQClient)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return base.rabbitMQClient.ExecuteAsync(stoppingToken);

        }
    }
}
