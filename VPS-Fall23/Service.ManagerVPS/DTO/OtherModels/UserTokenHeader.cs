namespace Service.ManagerVPS.DTO.OtherModels;

public class UserTokenHeader
{
    public string UserId { get; set; } = null!;
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public int RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public string? Avatar { get; set; }
    public DateTime Expires { get; set; }
    public DateTime ModifiedAt { get; set; }
}