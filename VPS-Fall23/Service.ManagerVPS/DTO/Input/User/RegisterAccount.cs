using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input.User;

public class RegisterAccount
{
    [Required(ErrorMessage = "Email cannot be null!")]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required(ErrorMessage = "Password cannot be null!")]
    [StringLength(12, MinimumLength = 6, ErrorMessage = "Password must be 6 - 12 characters!")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "FirstName cannot be null!")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "LastName cannot be null!")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "PhoneNumber cannot be null!")]
    public string PhoneNumber { get; set; } = null!;
}