using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class GlobalStatus
    {
        public GlobalStatus()
        {
            ContractLogs = new HashSet<ContractLog>();
            Contracts = new HashSet<Contract>();
            ParkingTransactions = new HashSet<ParkingTransaction>();
            ReportStatusNavigations = new HashSet<Report>();
            ReportTypeNavigations = new HashSet<Report>();
        }

        public int TypeId { get; set; }
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        public virtual Type Type { get; set; } = null!;
        public virtual ICollection<ContractLog> ContractLogs { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<ParkingTransaction> ParkingTransactions { get; set; }
        public virtual ICollection<Report> ReportStatusNavigations { get; set; }
        public virtual ICollection<Report> ReportTypeNavigations { get; set; }
    }
}
