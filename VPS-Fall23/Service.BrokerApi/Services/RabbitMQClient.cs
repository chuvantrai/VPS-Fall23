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
        protected readonly string queueOut;
        protected readonly ushort fetchNo;
        protected readonly IModel channel;
        public RabbitMQClient(string endpoint,
            int port,
            string username,
            string password,
            ushort fetchNo,
            string queueOut)
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
            this.queueOut = queueOut;
            channel = connection.CreateModel();
            _ = channel.QueueDeclare(queueOut, true, false, false, null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: fetchNo, global: false);
        }
        public RabbitMQClient(RabbitMQProfile rabbitMQProfile)
            : this(rabbitMQProfile.EndPoint,
                  rabbitMQProfile.Port,
                  rabbitMQProfile.Username,
                  rabbitMQProfile.Password,
                  rabbitMQProfile.FetchNo,
                  rabbitMQProfile.QueueIn)
        {
        }
        public RabbitMQClient(IOptions<RabbitMQProfile> options)
            :this(options.Value)
        {

        }
        public Task SendMessageAsync<T>(T message)
        {
            string jsonMsg = JsonSerializer.Serialize(message);
            var msgBytes = Encoding.UTF8.GetBytes(jsonMsg);
            channel.BasicPublish(string.Empty, string.Empty, null, msgBytes);
            return Task.CompletedTask;
        }

    }
}
