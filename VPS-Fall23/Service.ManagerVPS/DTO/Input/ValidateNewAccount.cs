using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class ValidateNewAccount
{
    [Required] 
    [EmailAddress] 
    public string Email { get; set; } = null!;

    [Required]
    public int VerifyCode { get; set; }
}