using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class PromoCodeParkingZone
    {
        public Guid PromoCodeId { get; set; }
        public Guid ParkingZoneId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }

        public virtual ParkingZone ParkingZone { get; set; } = null!;
        public virtual PromoCode PromoCode { get; set; } = null!;
    }
}
