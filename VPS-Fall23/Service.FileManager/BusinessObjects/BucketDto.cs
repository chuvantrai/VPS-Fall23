using System.ComponentModel.DataAnnotations;

namespace VPS.MinIO.BusinessObjects
{
    public class BucketDto
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Bucket can not be empty")]
        public string BucketName { get; set; } = null!;
    }
}
