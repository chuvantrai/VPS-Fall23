using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.BackgroundServices.ParkingZoneBackgroundServices;
using Service.WorkerVPS.BackgroundServices;
using Service.WorkerVPS.Brokers.ParkingZoneJobBrokers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.HostingBackgroundServices.ParkingZone
{
    internal class RemoveCancelBookingJobBackgroundService : QuartzBackgroundService
    {
        public RemoveCancelBookingJobBackgroundService(
            ILogger<RemoveCancelBookingJobBackgroundService> logger,
            QuartzServices quartzServices,
            RemoveCancelBookingDequeue rabbitMQClient)
            : base(rabbitMQClient, logger, quartzServices)
        {
        }
    }
}
