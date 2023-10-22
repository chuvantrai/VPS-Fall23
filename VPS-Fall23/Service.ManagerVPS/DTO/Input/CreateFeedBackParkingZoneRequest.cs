using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class CreateFeedBackParkingZoneRequest
{
    [Required]
    public Guid ParkingZoneId { get; set; }
    [StringLength(500)]
    public string? Content { get; set; }
    [Required]
    [RegularExpression("^[1-5]$")]
    public int Rate { get; set; }
}