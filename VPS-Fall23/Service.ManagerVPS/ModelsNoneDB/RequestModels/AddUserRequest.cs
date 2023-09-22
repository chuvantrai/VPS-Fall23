using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.ModelsNoneDB.RequestModels;

public class AddUserRequest
{
    [Required]
    public Guid Id { get; set; }
    
    [Required]
    [MaxLength(200)]
    public string Name { get; set; }
}