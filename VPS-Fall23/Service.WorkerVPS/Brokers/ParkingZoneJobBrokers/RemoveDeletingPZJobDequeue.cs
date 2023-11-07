using Microsoft.Extensions.Options;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Models;
using Service.WorkerVPS.Models.ParkingZoneJob;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.Brokers.ParkingZoneJobBrokers
{
    internal class RemoveDeletingPZJobDequeue : RabbitMQClient<Guid>
    {
        protected QuartzServices quartzServices;
        public RemoveDeletingPZJobDequeue(IOptions<RabbitMQProfile> rabbitMQProfileConfig, QuartzServices quartzServices)
            : base(rabbitMQProfileConfig.Value, rabbitMQProfileConfig.Value.QueueInfo.RemoveDeletingPZJobQueueName)
        {
            this.quartzServices = quartzServices;
        }
        public override async Task DequeueHandle(Guid absentId)
        {
            await quartzServices.Scheduler.DeleteJob(new Quartz.JobKey(absentId.ToString(), Extensions.Constant.ParkingZoneAbsentJobGroupName));
        }
    }
}
