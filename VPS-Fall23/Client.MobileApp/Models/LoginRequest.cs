using System.ComponentModel.DataAnnotations;

namespace Client.MobileApp.Models
{
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = null!;
        [StringLength(12, MinimumLength = 6)]
        public string Password { get; set; } = null!;
    }
}
