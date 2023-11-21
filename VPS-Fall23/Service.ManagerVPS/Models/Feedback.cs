namespace Service.ManagerVPS.Models
{
    public partial class Feedback
    {
        public Feedback()
        {
            InverseParent = new HashSet<Feedback>();
        }

        public Guid Id { get; set; }
        public int SubId { get; set; }
        public Guid ParkingZoneId { get; set; }
        public string? Content { get; set; }
        public int Rate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? Email { get; set; }
        public Guid? ParentId { get; set; }

        public virtual Feedback? Parent { get; set; }
        public virtual ParkingZone ParkingZone { get; set; } = null!;
        public virtual ICollection<Feedback> InverseParent { get; set; }
    }
}
