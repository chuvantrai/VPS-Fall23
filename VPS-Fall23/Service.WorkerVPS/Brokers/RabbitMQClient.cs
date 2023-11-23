using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Service.WorkerVPS.Models;
using System.Net;
using System.Text;
using System.Text.Json;

namespace Service.WorkerVPS.Brokers
{
    internal abstract class RabbitMQClient<TQueueMessageType> : IRabbitMQClient

    {
        protected IConnection connection;
        protected readonly string endpoint, username, password, queueOut;
        protected readonly ushort fetchNo;
        protected readonly int port;
        readonly ILogger<RabbitMQClient<TQueueMessageType>> logger;
        public RabbitMQClient(string endpoint,
            int port,
            string username,
            string password,
            ushort fetchNo,
            string queueOut,
            ILogger<RabbitMQClient<TQueueMessageType>> logger)
        {
            this.port = port;
            this.endpoint = endpoint;
            this.username = username;
            this.password = password;
            this.fetchNo = fetchNo;
            this.queueOut = queueOut;
            this.logger = logger;
        }
        public RabbitMQClient(RabbitMQProfile rabbitMQProfile, string queueOut, ILogger<RabbitMQClient<TQueueMessageType>> logger)
            : this(rabbitMQProfile.EndPoint,
                  rabbitMQProfile.Port,
                  rabbitMQProfile.Username,
                  rabbitMQProfile.Password,
                  rabbitMQProfile.FetchNo,
                  queueOut, logger)
        {
        }
        public Task Connect()
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



            return Task.CompletedTask;
        }

        public Task ExecuteAsync(CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                Console.WriteLine("Broker stopped");
                return Task.CompletedTask;
            }

            IModel channel = connection.CreateModel();
            _ = channel.QueueDeclare(queueOut, true, false, false, null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: fetchNo, global: false);
            EventingBasicConsumer consumer = new(channel);
            consumer.Received += Dequeue;
            channel.BasicConsume(queueOut, false, consumer);
            return Task.CompletedTask;
        }
        int retryTimes = 1;
        protected void Dequeue(object? sender, BasicDeliverEventArgs e)
        {
            IModel channel = ((EventingBasicConsumer)sender).Model;
            try
            {
                logger.Log(LogLevel.Information, "Received message...");
                string msgJson = Encoding.UTF8.GetString(e.Body.ToArray());
                TQueueMessageType message = JsonSerializer.Deserialize<TQueueMessageType>(msgJson);
                DequeueHandle(message).Wait();
                channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Information, $"Dequeue message exception: {ex.Message}");
                if (retryTimes == 3)
                {

                    channel.BasicAck(e.DeliveryTag, false);
                    retryTimes = 1;
                    return;
                }
                else
                {
                    retryTimes +=1;
                }
                channel.BasicNack(e.DeliveryTag, false, true);
            }

        }
        public abstract Task DequeueHandle(TQueueMessageType message);

    }
}
