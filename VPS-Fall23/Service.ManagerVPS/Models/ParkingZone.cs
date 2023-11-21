namespace Service.ManagerVPS.Models
{
    public partial class ParkingZone
    {
        public ParkingZone()
        {
            Contracts = new HashSet<Contract>();
            Feedbacks = new HashSet<Feedback>();
            ParkingTransactions = new HashSet<ParkingTransaction>();
            ParkingZoneAbsents = new HashSet<ParkingZoneAbsent>();
            ParkingZoneAttendants = new HashSet<ParkingZoneAttendant>();
            PromoCodeParkingZones = new HashSet<PromoCodeParkingZone>();
        }

        public Guid Id { get; set; }
        public int SubId { get; set; }
        public Guid CommuneId { get; set; }
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
        public decimal? Lat { get; set; }
        public decimal? Lng { get; set; }
        public TimeSpan WorkFrom { get; set; }
        public TimeSpan WorkTo { get; set; }
        public bool? IsFull { get; set; }

        public virtual Commune Commune { get; set; } = null!;
        public virtual ParkingZoneOwner Owner { get; set; } = null!;
        public virtual ICollection<Contract> Contracts { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<ParkingTransaction> ParkingTransactions { get; set; }
        public virtual ICollection<ParkingZoneAbsent> ParkingZoneAbsents { get; set; }
        public virtual ICollection<ParkingZoneAttendant> ParkingZoneAttendants { get; set; }
        public virtual ICollection<PromoCodeParkingZone> PromoCodeParkingZones { get; set; }
    }
}
