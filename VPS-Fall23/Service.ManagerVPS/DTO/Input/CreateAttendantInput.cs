using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class CreateAttendantInput
{
    [Required] 
    public Guid? ParkingZoneId { get; set; }
    
    [Required] 
    public string UserName { get; set; } = null!;

    [Required(ErrorMessage = "Password cannot be null!")]
    [StringLength(12, MinimumLength = 6, ErrorMessage = "Password must be 6 - 12 characters!")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "FirstName cannot be null!")]
    [MaxLength(20, ErrorMessage = "FirstName maximum 20 characters!")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "LastName cannot be null!")]
    [MaxLength(20, ErrorMessage = "LastName maximum 20 characters!")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "PhoneNumber cannot be null!")]
    [StringLength(10, MinimumLength = 10, ErrorMessage = "Phone maximum 10 characters!")]
    public string PhoneNumber { get; set; } = null!;

    public Guid? CommuneId { get; set; }

    public string? Address { get; set; }
}