using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class Commune
    {
        public Commune()
        {
            Accounts = new HashSet<Account>();
            ParkingZones = new HashSet<ParkingZone>();
        }

        public Guid Id { get; set; }
        public int SubId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public Guid DistrictId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public bool? IsBlock { get; set; }

        public virtual Account CreatedByNavigation { get; set; } = null!;
        public virtual District District { get; set; } = null!;
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<ParkingZone> ParkingZones { get; set; }
    }
}
