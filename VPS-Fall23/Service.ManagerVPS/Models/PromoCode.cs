using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class PromoCode
    {
        public PromoCode()
        {
            PromoCodeParkingZones = new HashSet<PromoCodeParkingZone>();
        }

        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid OwnerId { get; set; }
        public int Discount { get; set; }
        public int NumberOfUses { get; set; }

        public virtual ParkingZoneOwner Owner { get; set; } = null!;
        public virtual ICollection<PromoCodeParkingZone> PromoCodeParkingZones { get; set; }
    }
}
