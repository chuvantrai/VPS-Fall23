using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class ParkingTransactionDetail
    {
        public Guid Id { get; set; }
        public int SubId { get; set; }
        public Guid ParkingTransactionId { get; set; }
        public string Detail { get; set; } = null!;
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public decimal UnitPricePerHour { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual ParkingTransaction ParkingTransaction { get; set; } = null!;
    }
}
