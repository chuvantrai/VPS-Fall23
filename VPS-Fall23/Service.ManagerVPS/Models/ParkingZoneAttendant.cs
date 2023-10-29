namespace Service.ManagerVPS.Models
{
    public partial class ParkingZoneAttendant
    {
        public Guid Id { get; set; }
        public Guid ParkingZoneId { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual Account IdNavigation { get; set; } = null!;
        public virtual ParkingZone ParkingZone { get; set; } = null!;
    }
}
