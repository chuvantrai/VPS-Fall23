using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input
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
