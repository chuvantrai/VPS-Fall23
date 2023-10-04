namespace VPS.MinIO.BusinessObjects.AppSetting
{
    public class AppSetting
    {
        public MinIO MinIO { get; set; } = null!;
    }
    public class MinIO
    {
        public string EndPoint { get; set; } = null!;
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public Presign Presign { get; set; } = null!;
    }
    public class Presign
    {
        public int ExpireInSecond { get; set; }
    }
}
