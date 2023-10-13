using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input
{
    public class BookingSlot
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]
        public string LicensePlate { get; set; }
        [Required]
        public DateTime CheckinAt { get; set; }
        [Required]
        public DateTime CheckoutAt { get; set; }

        [Required]
        public Guid ParkingZoneId { get; set; }
    }
}
