using System.ComponentModel.DataAnnotations;
using Service.ManagerVPS.Constants.Enums;

namespace Service.ManagerVPS.DTO.Input;

public class CreateReportRequest
{
    [Required] [StringLength(1000)] 
    public string Content { get; set; } = null!;
    [Required]
    public ReportTypeEnum Type { get; set; }
    public Guid? UserId { get; set; }
    public string? Email { get; set; } = string.Empty;
    public string? Phone { get; set; } = string.Empty;
}