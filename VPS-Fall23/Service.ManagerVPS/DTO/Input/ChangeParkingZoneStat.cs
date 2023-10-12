using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class ChangeParkingZoneStat
{
    [Required]
    public Guid Id { get; set; }

    [Required]
    public bool IsApprove { get; set; }

    public string? RejectReason { get; set; }
}