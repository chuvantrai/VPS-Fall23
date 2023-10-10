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

        public virtual Account? CreatedByNavigation { get; set; }
    }
}
