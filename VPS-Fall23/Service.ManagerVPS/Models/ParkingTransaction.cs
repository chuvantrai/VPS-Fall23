using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class ParkingTransaction
    {
        public ParkingTransaction()
        {
            ParkingTransactionDetails = new HashSet<ParkingTransactionDetail>();
        }

        public Guid ParkingZoneId { get; set; }
        public Guid Id { get; set; }
        public int SubId { get; set; }
        public DateTime CheckinAt { get; set; }
        public DateTime? CheckoutAt { get; set; }
        public string LicensePlate { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public Guid CheckinBy { get; set; }
        public Guid CheckoutBy { get; set; }
        public int? StatusId { get; set; }
        public string? PaidTransactionId { get; set; }

        public virtual Account CheckinByNavigation { get; set; } = null!;
        public virtual Account CheckoutByNavigation { get; set; } = null!;
        public virtual ParkingZone ParkingZone { get; set; } = null!;
        public virtual GlobalStatus? Status { get; set; }
        public virtual ICollection<ParkingTransactionDetail> ParkingTransactionDetails { get; set; }
    }
}
