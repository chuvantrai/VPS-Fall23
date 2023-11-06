using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class ParkingZoneAbsent
    {
        public Guid Id { get; set; }
        public int SubId { get; set; }
        public Guid ParkingZoneId { get; set; }
        public DateTime From { get; set; }
        public DateTime? To { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Reason { get; set; } = null!;

        public virtual ParkingZone ParkingZone { get; set; } = null!;
    }
}
