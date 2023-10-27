using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class UpdateParkingZoneInput
{
    [Required]
    public Guid? ParkingZoneId { get; set; }

    [Required] 
    public string ParkingZoneName { get; set; } = null!;

    [Required]
    public decimal? PricePerHour { get; set; }

    [Required]
    public decimal? PriceOverTimePerHour { get; set; }

    [Required]
    public int? Slots { get; set; }

    [Required]
    public TimeSpan WorkFrom { get; set; }
    
    [Required]
    public TimeSpan WorkTo { get; set; }
    
    [Required]
    public Guid? CommuneId { get; set; }

    [Required] 
    public string DetailAddress { get; set; } = null!;

    [Required]
    public IFormFileCollection ParkingZoneImages { get; set; } = null!;
}