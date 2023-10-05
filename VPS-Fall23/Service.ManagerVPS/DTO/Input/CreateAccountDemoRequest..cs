using System.ComponentModel.DataAnnotations;
using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.DTO.Input;

public class CreateAccountDemoRequest
{
    [Required] public UserRoleEnum TypeId { get; set; }
    [Required] public string Username { get; set; } = null!;
    [Required] public string Password { get; set; } = null!;
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [StringLength(5,  ErrorMessage = "Email sai")] public string Email { get; set; } = null!;
    [Required] 
    [StringLength(10, MinimumLength = 10, ErrorMessage = "sđt sai")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "sđt sai")]
    public string PhoneNumber { get; set; } = null!;
}