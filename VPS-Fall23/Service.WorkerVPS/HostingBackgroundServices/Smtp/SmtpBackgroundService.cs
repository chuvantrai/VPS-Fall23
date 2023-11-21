using Service.WorkerVPS.Brokers.SmtpBrokers;

namespace Service.WorkerVPS.BackgroundServices.Smtp
{
    internal class SmtpBackgroundService : BackgroundService
    {
        public SmtpBackgroundService(SendMailDequeue rabbitMQClient, ILogger<BackgroundService> logger)
            : base(rabbitMQClient, logger)
        {
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.Log(LogLevel.Information, "Smtp service started");
            await base.StartAsync(cancellationToken);
        }
    }
}
