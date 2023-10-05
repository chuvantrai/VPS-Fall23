using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.DTO.OtherModels;

public class UserTokenHeader
{
    public Guid UserId { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public string? Avatar { get; set; }
}