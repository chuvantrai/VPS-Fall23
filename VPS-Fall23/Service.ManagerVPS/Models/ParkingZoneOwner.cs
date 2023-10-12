using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class ParkingZoneOwner
    {
        public ParkingZoneOwner()
        {
            ParkingZones = new HashSet<ParkingZone>();
        }

        public Guid Id { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Phone { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? Dob { get; set; }

        public virtual Account IdNavigation { get; set; } = null!;
        public virtual ICollection<ParkingZone> ParkingZones { get; set; }
    }
}
