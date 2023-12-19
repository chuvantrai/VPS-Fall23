using Microsoft.Extensions.Options;
using Quartz;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Models;
using Service.WorkerVPS.Models.ParkingZoneJob;
using Service.WorkerVPS.Services.SchedulerJobs.ParkingZone;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.WorkerVPS.Brokers.ParkingZoneJobBrokers
{
    internal class CreateCancelBookingDequeue : RabbitMQClient<AutoCancelBookingDto>
    {

        protected QuartzServices quartzServices;
        public CreateCancelBookingDequeue(IOptions<RabbitMQProfile> rabbitMQProfileConfig,
            QuartzServices quartzServices,
            ILogger<CreateCancelBookingDequeue> logger)
            : base(rabbitMQProfileConfig.Value, rabbitMQProfileConfig.Value.QueueInfo.CreateCancelBookingJobQueueName, logger)
        {
            this.quartzServices = quartzServices;
        }

        public override async Task DequeueHandle(AutoCancelBookingDto autoCancelBookingDto)
        {
            var job = JobBuilder.Create<AutoCancelBookingTransaction>()

                   .WithIdentity(autoCancelBookingDto.ParkingTransactionId.ToString(), Extensions.Constant.ParkingTransactionJobGroupName)
                   .UsingJobData("parkingTransactionId", autoCancelBookingDto.ParkingTransactionId)
                   .Build();
            // Tạo trigger để lên lịch công việc
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"trigger_{autoCancelBookingDto.ParkingTransactionId}", Extensions.Constant.ParkingZoneAbsentJobGroupName)
                .StartAt(autoCancelBookingDto.CancelAt)
                .Build();
            // Lên lịch công việc
          var result=  await quartzServices.Scheduler.ScheduleJob(job, trigger);
            Console.WriteLine($"Created quartz job with time offset: {result.ToString("yyyy/MM/dd HH:mm:ss")}");

        }
    }
}
