using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;

namespace Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices
{
    internal class RemoveDeletingPZJobBackGroundService : QuartzBackgroundService
    {
        public RemoveDeletingPZJobBackGroundService(
            ILogger<CreateDeletingPZJobBackgroundService> logger,
            QuartzServices quartzServices,
            RemoveDeletingPZJobDequeue rabbitMQClient)
            : base(rabbitMQClient,logger, quartzServices)
        {
        }
    }
}
