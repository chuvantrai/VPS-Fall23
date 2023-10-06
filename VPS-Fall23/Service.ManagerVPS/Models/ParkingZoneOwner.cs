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
        public string DetailAddress { get; set; } = null!;
        public Guid CommuneId { get; set; }
        public bool IsApproved { get; set; }
        public string? RejectedReason { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Commune Commune { get; set; } = null!;
        public virtual Account IdNavigation { get; set; } = null!;
        public virtual ICollection<ParkingZone> ParkingZones { get; set; }
    }
}
