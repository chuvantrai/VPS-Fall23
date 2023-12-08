using Service.WorkerVPS.Brokers;

namespace Service.WorkerVPS.BackgroundServices
{
    internal class BackgroundService : Microsoft.Extensions.Hosting.BackgroundService
    {
        protected readonly ILogger<BackgroundService> _logger;
        protected readonly IRabbitMQClient rabbitMQClient;
        public BackgroundService(IRabbitMQClient rabbitMQClient, ILogger<BackgroundService> logger)
        {
            this.rabbitMQClient = rabbitMQClient;
            this._logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
       
            await this.rabbitMQClient.ExecuteAsync(stoppingToken);
        }
        public override Task StartAsync(CancellationToken cancellationToken)
        {
            rabbitMQClient.Connect();
            return base.StartAsync(cancellationToken);
        }
    }
}
