using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class LoginRequest
{
    [Required]
    public string Username { get; set; } = null!;
    [StringLength(12, MinimumLength = 6)]
    public string Password { get; set; } = null!;
}