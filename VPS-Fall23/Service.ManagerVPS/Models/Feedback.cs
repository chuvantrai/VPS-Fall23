using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class Feedback
    {
        public Guid Id { get; set; }
        public int SubId { get; set; }
        public Guid ParkingZoneId { get; set; }
        public string? Content { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Email { get; set; }

        public virtual ParkingZone ParkingZone { get; set; } = null!;
    }
}
