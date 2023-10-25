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
    [RegularExpression("^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\\.[a-zA-Z0-9-.]+$")]
    public string? Email { get; set; } = string.Empty;
    [StringLength(10)]
    [RegularExpression("^$|^[0-9]+$")]
    public string? Phone { get; set; } = string.Empty;

    public string? PaymentCode { get; set; } = string.Empty;
}