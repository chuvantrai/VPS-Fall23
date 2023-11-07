using System.ComponentModel.DataAnnotations;

namespace Service.ManagerVPS.DTO.Input;

public class AddReplyToFeedbackInput
{
    [Required]
    public Guid? FeedbackId { get; set; }

    [Required]
    public string Content { get; set; } = null!;
}