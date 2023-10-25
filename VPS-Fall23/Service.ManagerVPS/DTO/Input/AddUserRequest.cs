using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class AddUserRequest
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; } = null!;
}