using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
