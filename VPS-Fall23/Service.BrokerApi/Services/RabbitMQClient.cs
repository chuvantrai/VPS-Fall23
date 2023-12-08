using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Service.BrokerApi.Models;
using System.Text;
using System.Text.Json;

namespace Service.BrokerApi.Services
{
    public class RabbitMQClient : IRabbitMQClient
    {
        protected readonly IConnection connection;
        protected readonly ushort fetchNo;
        protected readonly IModel channel;
        public RabbitMQClient(string endpoint,
            int port,
            string username,
            string password,
            ushort fetchNo)
        {
            var connectionFactory = new ConnectionFactory()
            {
                HostName = endpoint,
                Port = port,
                UserName = username,
                Password = password,
                VirtualHost = "/"
            };
            connection = connectionFactory.CreateConnection();
            this.fetchNo = fetchNo;
            channel = connection.CreateModel();
            channel.BasicQos(prefetchSize: 0, prefetchCount: fetchNo, global: false);
        }
        public RabbitMQClient(RabbitMQProfile rabbitMQProfile)
            : this(rabbitMQProfile.EndPoint,
                  rabbitMQProfile.Port,
                  rabbitMQProfile.Username,
                  rabbitMQProfile.Password,
                  rabbitMQProfile.FetchNo)
        {
        }
        public RabbitMQClient(IOptions<RabbitMQProfile> options)
            :this(options.Value)
        {

        }
        public Task SendMessageAsync<T>(string queueIn, T message)
        {
            _ = channel.QueueDeclare(queueIn, true, false, false, null);
          
            string jsonMsg = JsonSerializer.Serialize(message);
            var msgBytes = Encoding.UTF8.GetBytes(jsonMsg);
            channel.BasicPublish("", queueIn, null, msgBytes);
            return Task.CompletedTask;
        }

    }
}
