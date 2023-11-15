using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class Report
    {
        public Guid Id { get; set; }
        public int SubId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedAt { get; set; }
        public Guid? CreatedBy { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? PaymentCode { get; set; }

        public virtual Account? CreatedByNavigation { get; set; }
        public virtual GlobalStatus StatusNavigation { get; set; } = null!;
        public virtual GlobalStatus TypeNavigation { get; set; } = null!;
    }
}
