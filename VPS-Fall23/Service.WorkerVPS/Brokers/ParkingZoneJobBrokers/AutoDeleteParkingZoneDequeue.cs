
using Quartz;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Models;
using Service.WorkerVPS.Models.ParkingZoneJob;
using Service.WorkerVPS.Scheduler.ParkingZoneSchedulerJobs;

namespace Service.WorkerVPS.Brokers.ParkingZoneJobBrokers
{
    internal class AutoDeleteParkingZoneDequeue : RabbitMQClient<AutoDeleteParkingZoneDto>
    {
        protected QuartzServices quartzServices;
        public AutoDeleteParkingZoneDequeue(RabbitMQProfile rabbitMQProfile, QuartzServices quartzServices)
            : base(rabbitMQProfile)
        {
            this.quartzServices = quartzServices;
        }

        public override async Task DequeueHandle(AutoDeleteParkingZoneDto autoDeleteParkingZoneDto)
        {
            var job = JobBuilder.Create<AutoDeleteParkingZoneJob>()
                   .WithIdentity($"auto_delete_{autoDeleteParkingZoneDto.ParkingZoneId}", "parkingZoneScheduler")
                   .UsingJobData("parkingZoneId", autoDeleteParkingZoneDto.ParkingZoneId)
                   .Build();
            // Tạo trigger để lên lịch công việc
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"auto_delete_{autoDeleteParkingZoneDto.ParkingZoneId}", "parkingZoneScheduler")
                .StartAt(autoDeleteParkingZoneDto.DeleteAt)
                .Build();

            // Lên lịch công việc
            await quartzServices.Scheduler.ScheduleJob(job, trigger);

        }
    }
}
