using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class PromoCodeInformation
    {
        public PromoCodeInformation()
        {
            PromoCodes = new HashSet<PromoCode>();
        }

        public Guid Id { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid OwnerId { get; set; }
        public int Discount { get; set; }
        public bool IsSent { get; set; }

        public virtual ParkingZoneOwner Owner { get; set; } = null!;
        public virtual ICollection<PromoCode> PromoCodes { get; set; }
    }
}
