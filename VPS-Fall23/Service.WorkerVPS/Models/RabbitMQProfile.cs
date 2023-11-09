namespace Service.WorkerVPS.Models
{
    public class RabbitMQProfile
    {
        public string EndPoint { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public QueueInfo QueueInfo { get; set; } = null!;
        public ushort FetchNo { get; set; }
    }
    public class QueueInfo
    {
        public string CreateDeletingPZJobQueueName { get; set; } = null!;
        public string RemoveDeletingPZJobQueueName { get; set; } = null!;
    }
}
