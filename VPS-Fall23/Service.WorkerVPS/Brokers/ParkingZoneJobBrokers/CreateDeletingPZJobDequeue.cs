
using Microsoft.Extensions.Options;
using Quartz;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Models;
using Service.WorkerVPS.Models.ParkingZoneJob;
using Service.WorkerVPS.Scheduler.ParkingZoneSchedulerJobs;

namespace Service.WorkerVPS.Brokers.ParkingZoneJobBrokers
{
    internal class CreateDeletingPZJobDequeue : RabbitMQClient<AutoDeleteParkingZoneDto>
    {
        protected QuartzServices quartzServices;
        public CreateDeletingPZJobDequeue(IOptions<RabbitMQProfile> rabbitMQProfileConfig, QuartzServices quartzServices)
            : base(rabbitMQProfileConfig.Value, rabbitMQProfileConfig.Value.QueueInfo.CreateDeletingPZJobQueueName)
        {
            this.quartzServices = quartzServices;
        }

        public override async Task DequeueHandle(AutoDeleteParkingZoneDto autoDeleteParkingZoneDto)
        {
            var job = JobBuilder.Create<AutoDeleteParkingZoneJob>()

                   .WithIdentity(autoDeleteParkingZoneDto.ParkingZoneAbsentId.ToString(), Extensions.Constant.ParkingZoneAbsentJobGroupName)
                   .UsingJobData("parkingZoneId", autoDeleteParkingZoneDto.ParkingZoneId)
                   .Build();
            // Tạo trigger để lên lịch công việc
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"trigger_{autoDeleteParkingZoneDto.ParkingZoneAbsentId}", Extensions.Constant.ParkingZoneAbsentJobGroupName)
                .StartAt(autoDeleteParkingZoneDto.DeleteAt)
                .Build();
            // Lên lịch công việc
            await quartzServices.Scheduler.ScheduleJob(job, trigger);

        }
    }
}
