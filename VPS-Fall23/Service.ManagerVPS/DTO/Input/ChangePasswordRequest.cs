using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class ChangePasswordRequest
{
    [StringLength(12, MinimumLength = 6, ErrorMessage = "OldPassword length minimum 6 and maximum 12 characters!")]
    public string OldPassword { get; set; } = null!;

    [StringLength(12, MinimumLength = 6, ErrorMessage = "NewPassword length minimum 6 and maximum 12 characters!")]
    public string NewPassword { get; set; } = null!;
}