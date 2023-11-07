using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class City
    {
        public City()
        {
            Districts = new HashSet<District>();
        }

        public Guid Id { get; set; }
        public int SubId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual Account CreatedByNavigation { get; set; } = null!;
        public virtual ICollection<District> Districts { get; set; }
    }
}
