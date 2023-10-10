using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class SendCodeForgotPasswordRequest
{
    [Required] public string UserName { get; set; } = null!;
}