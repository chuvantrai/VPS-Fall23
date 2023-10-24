using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class ChangeParkingZoneFullStatus
{
    [Required]
    public Guid? ParkingZoneId { get; set; }

    [Required]
    public bool? IsFull { get; set; }
}