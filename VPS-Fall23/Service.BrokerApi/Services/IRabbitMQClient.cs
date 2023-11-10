namespace Service.BrokerApi.Services
{
    public interface IRabbitMQClient
    {
        Task SendMessageAsync<T>(string queueIn, T message);
    }
}
