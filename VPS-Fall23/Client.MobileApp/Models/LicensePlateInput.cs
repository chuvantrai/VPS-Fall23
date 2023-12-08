using System.ComponentModel.DataAnnotations;

namespace Client.MobileApp.Models
{
    public class LicensePlateInput
    {
        [Required]
        public DateTime CheckAt { get; set; }

        [Required]
        public Guid CheckBy { get; set; }

        public string LicensePlate { get; set; }
    }
}
