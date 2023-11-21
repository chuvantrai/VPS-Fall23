namespace Service.WorkerVPS.Models
{
    public class HostEmailProfile
    {
        public string Host { get; set; } = null!;
        public int Port { get; set; }
        public string Account { get; set; } = null!;
        public string Password { get; set; } = null!;
        public bool UseSSL { get; set; }
    }
}
