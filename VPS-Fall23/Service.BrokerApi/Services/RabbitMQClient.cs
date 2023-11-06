using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.BrokerApi.Models;

namespace Service.BrokerApi.Services
{
    public abstract class RabbitMQClient
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

        public abstract Task ExcuteAsync<T>(T message);

    }
}
