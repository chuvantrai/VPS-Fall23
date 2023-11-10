using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices
{
    internal class RemoveDeletingPZJobBackGroundService : BackgroundService
    {
        public RemoveDeletingPZJobBackGroundService(
            ILogger<CreateDeletingPZJobBackgroundService> logger,
            QuartzServices quartzServices,
            RemoveDeletingPZJobDequeue rabbitMQClient)
            : base(logger, quartzServices, rabbitMQClient)
        {
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return base.rabbitMQClient.ExecuteAsync(stoppingToken);

        }
    }
}
