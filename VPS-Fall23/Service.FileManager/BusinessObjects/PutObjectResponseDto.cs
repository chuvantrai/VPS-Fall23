namespace VPS.MinIO.BusinessObjects
{
    public class PutObjectResponseDto
    {
        public long Size { get; set; }
        public string ObjectName { get; set; } = null!;
        public string ETag { get; set; } = null!;
    }
}
