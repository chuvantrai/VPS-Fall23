using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input
{
    public class UpdateParkingZoneAddressInput
    {
        [Required]
        public Guid ParkingZoneId {  get; set; }
        [Required]
        public string DetailAddress { get; set; } = null!;
        [Required]
        public DTO.GoongMap.Position Location { get; set; } = null!;
    }
}
