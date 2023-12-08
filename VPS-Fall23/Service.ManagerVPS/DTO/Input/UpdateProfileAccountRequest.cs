using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class UpdateProfileAccountRequest
{
    [Required] public string FirstName { get; set; } = null!;
    [Required] public string LastName { get; set; } = null!;
    [RegularExpression("^[0-9]+$")] public string PhoneNumber { get; set; } = null!;

    [MaxLength(100, ErrorMessage = "Address maximum 100 characters!")]
    public string? Address { get; set; } = null!;

    public Guid? CommuneId { get; set; }
    public Guid? AccountId { get; set; }
    public IFormFileCollection? AvatarImages { get; set; } = null!;
    public string? PathImage { get; set; }
}