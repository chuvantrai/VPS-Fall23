using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.WorkerVPS.Models;
using System.Text;
using System.Text.Json;

namespace Service.WorkerVPS.Brokers
{
    internal abstract class RabbitMQClient<TQueueMessageType> : IRabbitMQClient
        where TQueueMessageType : class
       
    {
        protected readonly IConnection connection;
        protected readonly string queueOut;
        protected readonly ushort fetchNo;
        protected readonly IModel channel;
        protected readonly EventingBasicConsumer consumer;
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
            consumer = new(channel);
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

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Broker stopped");
                return Task.CompletedTask;
            }
            consumer.Received += Dequeue;
            return Task.CompletedTask;
        }
        protected void Dequeue(object? sender, BasicDeliverEventArgs e)
        {
            string msgJson = Encoding.UTF8.GetString(e.Body.ToArray());
            TQueueMessageType message = JsonSerializer.Deserialize<TQueueMessageType>(msgJson);
            DequeueHandle(message).Wait();
        }
        public abstract Task DequeueHandle(TQueueMessageType message);

    }
}
