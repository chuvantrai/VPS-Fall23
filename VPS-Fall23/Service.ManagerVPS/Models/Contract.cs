using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class Contract
    {
        public Contract()
        {
            ContractLogs = new HashSet<ContractLog>();
        }

        public Guid Id { get; set; }
        public int SubId { get; set; }
        public Guid ParkingZoneId { get; set; }
        public string ContractCode { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public DateTime? SignedAt { get; set; }
        public int Status { get; set; }
        public DateTime PdfSavedAt { get; set; }
        public string? PaidTransactionId { get; set; }

        public virtual ParkingZone ParkingZone { get; set; } = null!;
        public virtual GlobalStatus StatusNavigation { get; set; } = null!;
        public virtual ICollection<ContractLog> ContractLogs { get; set; }
    }
}
