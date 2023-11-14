using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class BlockUserAccountInput
{
    [Required]
    public Guid? AccountId { get; set; }

    [Required]
    public bool? IsBlock { get; set; }

    public string? BlockReason { get; set; }
}