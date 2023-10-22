namespace Service.ManagerVPS.DTO.Output;

public class GetAccountProfileResponse
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public Guid? City { get; set; }
    public Guid? District { get; set; }
    public Guid? Commune { get; set; }
    public string? Address { get; set; }
    public string Role { get; set; } = null!;
    public DateTime? Dob { get; set; }
    public string? Avatar { get; set; }
    public string[]? AddressArray { get; set; }
    public int RoleId { get; set; }
}