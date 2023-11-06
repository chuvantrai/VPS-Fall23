namespace Service.BrokerApi.Services
{
    public interface IRabbitMQClient
    {
        Task SendMessageAsync<T>(T message);
    }
}
