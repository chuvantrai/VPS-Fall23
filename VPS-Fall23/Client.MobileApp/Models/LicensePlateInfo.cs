using System.ComponentModel.DataAnnotations;

namespace Client.MobileApp.Models
{
    public class LicensePlateInfo
    {
        [Required]
        public Google.Cloud.Vision.V1.Image LicensePlate { get; set; }

        [Required]
        public DateTime CheckAt { get; set; }

        [Required]
        public Guid CheckBy { get; set; }
    }
}
