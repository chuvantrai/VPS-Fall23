namespace Service.ManagerVPS.DTO.Input;

public class UpdateProfileAccountRequest
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string PhoneNumber { get; set; } = null!;
    public string? Address { get; set; }
    public Guid CommuneId { get; set; }
    public Guid? AccountId { get; set; }
    
    public IFormFileCollection AvatarImages { get; set; } = null!;
}