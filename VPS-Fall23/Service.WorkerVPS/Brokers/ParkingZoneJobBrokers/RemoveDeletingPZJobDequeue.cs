using Microsoft.Extensions.Options;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Models;

namespace Service.WorkerVPS.Brokers.ParkingZoneJobBrokers
{
    internal class RemoveDeletingPZJobDequeue : RabbitMQClient<Guid>
    {
        protected QuartzServices quartzServices;
        public RemoveDeletingPZJobDequeue(IOptions<RabbitMQProfile> rabbitMQProfileConfig, QuartzServices quartzServices,
            ILogger<RemoveDeletingPZJobDequeue> logger)
            : base(rabbitMQProfileConfig.Value, rabbitMQProfileConfig.Value.QueueInfo.RemoveDeletingPZJobQueueName, logger)
        {
            this.quartzServices = quartzServices;
        }
        public override async Task DequeueHandle(Guid absentId)
        {
            await quartzServices.Scheduler.DeleteJob(new Quartz.JobKey(absentId.ToString(), Extensions.Constant.ParkingZoneAbsentJobGroupName));
        }
    }
}
