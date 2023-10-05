using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class LoginRequest
{
    [Required(ErrorMessage = "Sai tài khoản")]
    public string Username { get; set; } = null!;
    [StringLength(12, MinimumLength = 6, ErrorMessage = "Mật khẩu 6 đến 12 ký tự")]
    public string Password { get; set; } = null!;
}