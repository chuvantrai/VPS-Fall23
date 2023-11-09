using Service.ManagerVPS.Extensions.StaticLogic.Scheduler;
using Service.WorkerVPS.Brokers;

namespace Service.WorkerVPS.BackgroundServices
{
    internal abstract class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        private readonly ILogger<BackgroundService> _logger;
        protected readonly QuartzServices quartzServices;
        protected readonly IRabbitMQClient rabbitMQClient;
        public BackgroundService(ILogger<BackgroundService> logger, QuartzServices quartzServices, IRabbitMQClient rabbitMQClient)
        {
            _logger = logger;
            this.quartzServices = quartzServices;
            this.rabbitMQClient = rabbitMQClient;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await quartzServices.Start();
            _logger.Log(LogLevel.Information, "Quartz service started");
            await base.StartAsync(cancellationToken);
        }
    }
}
