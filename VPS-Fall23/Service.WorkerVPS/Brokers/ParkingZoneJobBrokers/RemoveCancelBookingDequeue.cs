using Microsoft.Extensions.Options;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.Brokers.ParkingZoneJobBrokers
{
    internal class RemoveCancelBookingDequeue : RabbitMQClient<Guid>
    {
        protected QuartzServices quartzServices;
        public RemoveCancelBookingDequeue(IOptions<RabbitMQProfile> rabbitMQProfileConfig, QuartzServices quartzServices,
            ILogger<RemoveCancelBookingDequeue> logger)
            : base(rabbitMQProfileConfig.Value, rabbitMQProfileConfig.Value.QueueInfo.RemoveCancelBookingJobQueueName, logger)
        {
            this.quartzServices = quartzServices;
        }
        public override async Task DequeueHandle(Guid parkingTransactionId)
        {
            await quartzServices.Scheduler.DeleteJob(new Quartz.JobKey(parkingTransactionId.ToString(), Extensions.Constant.ParkingTransactionJobGroupName));
            Console.WriteLine("Deleted auto cancel booking job");
        }
    }
}
