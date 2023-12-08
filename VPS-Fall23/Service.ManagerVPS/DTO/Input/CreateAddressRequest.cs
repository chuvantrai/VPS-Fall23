using System.ComponentModel.DataAnnotations;
using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.DTO.Input;

public class CreateAddressRequest
{
    [Required]
    public AddressTypeEnum Type { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    public Guid? City { get; set; }
    public Guid? District { get; set; }
    [Required]
    public int Code { get; set; }
}