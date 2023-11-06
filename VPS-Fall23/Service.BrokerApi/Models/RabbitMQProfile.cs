namespace Service.BrokerApi.Models
{
    public class RabbitMQProfile
    {
        public string EndPoint { get; set; } = null!;
        public int Port { get; set; }
        public string Username { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string QueueIn { get; set; } = null!;
        public ushort FetchNo { get; set; }
    }
}
