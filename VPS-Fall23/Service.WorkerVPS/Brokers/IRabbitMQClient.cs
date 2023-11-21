namespace Service.WorkerVPS.Brokers
{
    internal interface IRabbitMQClient
    {
        Task Connect();
        Task ExecuteAsync(CancellationToken cancellationToken);
    }
}
