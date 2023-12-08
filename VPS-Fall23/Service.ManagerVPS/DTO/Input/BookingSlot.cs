using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input
{
    public class BookingSlot
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;
        [Required]
        [Phone]
        public string Phone { get; set; } = null!;
        [Required]
        public string LicensePlate { get; set; } = null!;
        [Required]
        public DateTime CheckinAt { get; set; }
        [Required]
        public DateTime CheckoutAt { get; set; }

        [Required]
        public Guid ParkingZoneId { get; set; }
        public string? PromoCode { get; set; }
    }
}
