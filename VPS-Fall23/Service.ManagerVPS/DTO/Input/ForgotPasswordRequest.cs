using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class ForgotPasswordRequest
{
    [Required] public string UserName { get; set; } = null!;
    [Required] public int VerifyCode { get; set; }
    [StringLength(12, MinimumLength = 6, ErrorMessage = "Password length minimum 6 and maximum 12 characters!")] 
    public string Password { get; set; } = null!;
}