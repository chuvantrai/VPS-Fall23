using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers;

namespace Service.WorkerVPS.BackgroundServices
{
    internal abstract class QuartzBackgroundService : BackgroundService
    {

        protected readonly QuartzServices quartzServices;

        public QuartzBackgroundService(
            IRabbitMQClient rabbitMQClient,
            ILogger<QuartzBackgroundService> logger,
            QuartzServices quartzServices)
            : base(rabbitMQClient, logger)
        {
            this.quartzServices = quartzServices;

        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await quartzServices.Start();
            _logger.Log(LogLevel.Information, "Quartz service started");
            await base.StartAsync(cancellationToken);
        }
    }
}
