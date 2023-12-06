using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using NetTopologySuite.Geometries;
using Service.ManagerVPS.Extensions.DbContext;

namespace Service.ManagerVPS.Models
{
    public partial class ParkingZone : SoftDeleteEntity
    {
        public ParkingZone()
        {
            Contracts = new HashSet<Contract>();
            Feedbacks = new HashSet<Feedback>();
            ParkingTransactions = new HashSet<ParkingTransaction>();
            ParkingZoneAbsents = new HashSet<ParkingZoneAbsent>();
            ParkingZoneAttendants = new HashSet<ParkingZoneAttendant>();
            PromoCodes = new HashSet<PromoCode>();
        }

        public Guid Id { get; set; }
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SubId { get; set; }
        public Guid? CommuneId { get; set; }
        public string Name { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
        public Guid OwnerId { get; set; }
        public string DetailAddress { get; set; } = null!;
        public decimal PricePerHour { get; set; }
        public decimal PriceOverTimePerHour { get; set; }
        public bool? IsApprove { get; set; }
        public string? RejectReason { get; set; }
        public int? Slots { get; set; }
        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public bool? IsFull { get; set; }
        public Geometry? Location { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public bool IsDelete { get; set; }

        public virtual Commune? Commune { get; set; }
        public virtual ParkingZoneOwner Owner { get; set; } = null!;
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<ParkingTransaction> ParkingTransactions { get; set; }
        public virtual ICollection<ParkingZoneAbsent> ParkingZoneAbsents { get; set; }
        public virtual ICollection<ParkingZoneAttendant> ParkingZoneAttendants { get; set; }
        public virtual ICollection<PromoCode> PromoCodes { get; set; }
    }
}
