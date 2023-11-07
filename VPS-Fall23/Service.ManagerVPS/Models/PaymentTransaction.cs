using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class PaymentTransaction
    {
        public Guid BookingId { get; set; }
        public int? TransactionNo { get; set; }
        public decimal Amount { get; set; }
        public string? BankCode { get; set; }
        public string? BankTranNo { get; set; }
        public string? CardType { get; set; }
        public DateTime? PayDate { get; set; }
        public string OrderInfo { get; set; } = null!;
        public int? ResponseCode { get; set; }
        public int? TransactionStatus { get; set; }
        public string TxnRef { get; set; } = null!;
        public string? SecureHashType { get; set; }
        public string SecureHash { get; set; } = null!;
        public string? ConnectionId { get; set; }

        public virtual ParkingTransaction Booking { get; set; } = null!;
    }
}
