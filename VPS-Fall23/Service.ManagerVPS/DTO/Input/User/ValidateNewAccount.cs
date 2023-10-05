using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input.User;

public class ValidateNewAccount
{
    [Required] public string Email { get; set; }

    [Required] public int VerifyCode { get; set; }
}