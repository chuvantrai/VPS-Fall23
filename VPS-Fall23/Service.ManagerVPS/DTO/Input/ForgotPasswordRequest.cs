using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class ForgotPasswordRequest
{
    [Required] public string UserName { get; set; } = null!;
    [Required] public int VerifyCode { get; set; }
    [StringLength(12, MinimumLength = 6)] public string Password { get; set; } = null!;
}