using Quartz;
using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;

namespace Service.WorkerVPS
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        readonly QuartzServices quartzServices;
        public Worker(ILogger<Worker> logger, QuartzServices quartzServices)
        {
            _logger = logger;
            this.quartzServices = quartzServices;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await quartzServices.Start();
            await base.StartAsync(cancellationToken);
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var job = JobBuilder.Create<DeleteParkingZoneJob>()
      .WithIdentity($"deleteParkingLotJob-{Guid.NewGuid()}", "group1")
      .UsingJobData("parkingZoneId", Guid.NewGuid())
      .Build();
            // Tạo trigger để lên lịch công việc sau 5 ngày
            var trigger = TriggerBuilder.Create()
                .WithIdentity($"deleteParkingLotTrigger-{Guid.NewGuid()}", "group1")
                .StartAt(DateTimeOffset.Now.Add(TimeSpan.FromMinutes(1)))
                .Build();

            // Lên lịch công việc
            await quartzServices.Scheduler.ScheduleJob(job, trigger);


        }
    }
}