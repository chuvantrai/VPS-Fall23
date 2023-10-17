using System.ComponentModel.DataAnnotations;

namespace Client.MobileApp.Models
{
    public class CheckLicensePlate
    {
        [Required]
        public string LicensePlate { get; set; } = null!;

        [Required]
        public DateTime CheckAt { get; set; }

        [Required]
        public Guid CheckBy { get; set; }
    }
}
