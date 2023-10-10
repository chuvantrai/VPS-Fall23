using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class ChangePasswordRequest
{
    [StringLength(12, MinimumLength = 6)] public string OldPassword { get; set; } = null!;
    [StringLength(12, MinimumLength = 6)] public string NewPassword { get; set; } = null!;
}