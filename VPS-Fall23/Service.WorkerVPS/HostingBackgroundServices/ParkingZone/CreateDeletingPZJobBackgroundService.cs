using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;

namespace Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices
{
    internal class CreateDeletingPZJobBackgroundService : QuartzBackgroundService
    {

        public CreateDeletingPZJobBackgroundService(
            ILogger<CreateDeletingPZJobBackgroundService> logger,
            QuartzServices quartzServices,
            CreateDeletingPZJobDequeue rabbitMQClient)
            : base(rabbitMQClient,logger, quartzServices)
        {
        }
    }
}
