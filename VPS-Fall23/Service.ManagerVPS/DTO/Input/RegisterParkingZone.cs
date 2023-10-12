using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class RegisterParkingZone
{
    [Required]
    public Guid OwnerId { get; set; }

    [Required]
    public string Name { get; set; } = null!;

    [Required]
    public decimal PricePerHour { get; set; }

    [Required]
    public decimal PriceOverTimePerHour { get; set; }

    [Required]
    public int Slots { get; set; }

    [Required]
    public Guid CommuneId { get; set; }

    [Required]
    public string DetailAddress { get; set; } = null!;
    
    [Required]
    public IFormFileCollection ParkingZoneImages { get; set; } = null!;
}