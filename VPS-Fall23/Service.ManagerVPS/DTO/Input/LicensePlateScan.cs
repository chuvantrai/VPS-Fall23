using Google.Cloud.Vision.V1;
using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input
{
    public class LicensePlateScan
    {
        [Required]
        public DateTime CheckAt { get; set; }

        [Required]
        public Guid CheckBy { get; set; }

        public byte[]? Image { get; set; }
    }
}
