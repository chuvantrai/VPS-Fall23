using System;
using System.Collections.Generic;

namespace Service.ManagerVPS.Models
{
    public partial class District
    {
        public District()
        {
            Communes = new HashSet<Commune>();
        }

        public Guid Id { get; set; }
        public int SubId { get; set; }
        public int Code { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid CityId { get; set; }
        public Guid CreatedBy { get; set; }

        public virtual City City { get; set; } = null!;
        public virtual Account CreatedByNavigation { get; set; } = null!;
        public virtual ICollection<Commune> Communes { get; set; }
    }
}
