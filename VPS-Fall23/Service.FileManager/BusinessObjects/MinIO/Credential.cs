using System.ComponentModel.DataAnnotations;

namespace VPS.MinIO.BusinessObjects.MinIO
{
    public class Credential
    {
        [Required]
        public string AccessKey { get; set; } = null!;
        [Required]
        public string SecretKey { get; set; } = null!;
    }
}
