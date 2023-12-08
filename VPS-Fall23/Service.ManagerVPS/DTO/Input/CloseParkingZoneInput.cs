using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class CloseParkingZoneInput
{
    [Required]
    public Guid? ParkingZoneId { get; set; }

    [Required] 
    public string Reason { get; set; } = null!;

    [Required]
    public DateTime? CloseFrom { get; set; }
    
    public DateTime? CloseTo { get; set; }
}