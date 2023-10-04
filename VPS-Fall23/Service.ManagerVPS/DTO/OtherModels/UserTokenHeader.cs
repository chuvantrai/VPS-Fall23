namespace Service.ManagerVPS.DTO.OtherModels;

public class UserTokenHeader
{
    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Email { get; set; }

    public string? Role { get; set; }

    public string? IpAddress { get; set; }
}