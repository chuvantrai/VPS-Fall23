using System.ComponentModel.DataAnnotations;
using System.Net.Mail;

namespace Service.WorkerVPS.Models.SmtpJob
{
    internal class SmtpMessageDto
    {
        [Required]
        public string Message { get; set; } = null!;
        [Required]
        public string Subject { get; set; } = null!;
        public string[] Receivers { get; set; } = null!;
        public Attachment[]? Attachments { get; set; }
    }
}
