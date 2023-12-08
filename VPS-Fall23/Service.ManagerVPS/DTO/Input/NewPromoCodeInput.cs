using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class NewPromoCodeInput
{
    [Required]
    public Guid? OwnerId { get; set; }

    [Required]
    public int? Discount { get; set; }

    [Required] 
    public List<Guid> ParkingZoneIds { get; set; }

    [Required]
    public DateTime? FromDate { get; set; }

    [Required]
    public DateTime? ToDate { get; set; }
}