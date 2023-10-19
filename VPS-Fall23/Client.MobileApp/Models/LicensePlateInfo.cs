using System.ComponentModel.DataAnnotations;

namespace Client.MobileApp.Models
{
    public class LicensePlateInfo
    {
        [Required]
        public DateTime CheckAt { get; set; }

        [Required]
        public Guid CheckBy { get; set; }

        public byte[] Image { get; set; }
    }
}
