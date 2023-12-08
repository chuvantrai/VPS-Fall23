using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class PromoCode
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public Guid PromoCodeInformationId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public int NumberOfUses { get; set; }
        public string UserEmail { get; set; } = null!;
        public string UserPhone { get; set; } = null!;
        public Guid ParkingZoneId { get; set; }
        public bool? UserReceivedCode { get; set; }

        public virtual ParkingZone ParkingZone { get; set; } = null!;
        public virtual PromoCodeInformation PromoCodeInformation { get; set; } = null!;
    }
}
