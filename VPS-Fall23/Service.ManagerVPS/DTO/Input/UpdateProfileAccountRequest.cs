using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class UpdateProfileAccountRequest
{
    [Required]
    [MaxLength(20, ErrorMessage = "FirstName maximum 20 characters!")]
    public string FirstName { get; set; } = null!;
    [Required]
    [MaxLength(20, ErrorMessage = "LastName maximum 20 characters!")]
    public string LastName { get; set; } = null!;
    [Required(ErrorMessage = "PhoneNumber Invalid!")]
    [RegularExpression("^[0-9]+$", ErrorMessage = "PhoneNumber Invalid!")]
    [MaxLength(10, ErrorMessage = "Phone maximum 10 number!")]
    public string PhoneNumber { get; set; } = null!;
    [MaxLength(100, ErrorMessage = "Address maximum 100 characters!")]
    public string? Address { get; set; } = null!;
    public Guid? CommuneId { get; set; }
    public Guid? AccountId { get; set; }
    public IFormFileCollection? AvatarImages { get; set; } = null!;
    public string? PathImage { get; set; }
}