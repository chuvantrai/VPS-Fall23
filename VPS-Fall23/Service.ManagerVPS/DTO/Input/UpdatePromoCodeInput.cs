using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class UpdatePromoCodeInput
{
    [Required]
    public Guid? PromoCodeId { get; set; }

    [Required]
    public string PromoCode { get; set; } = null!;

    [Required]
    public DateTime? FromDate { get; set; }

    [Required]
    public DateTime? ToDate { get; set; }

    [Required] 
    public List<Guid> ParkingZoneIds { get; set; } = new();
}