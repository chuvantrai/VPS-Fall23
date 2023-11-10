namespace Service.WorkerVPS.Models.ParkingZoneJob
{
    internal class AutoDeleteParkingZoneDto
    {
        public Guid ParkingZoneAbsentId { get; set; }
        public Guid ParkingZoneId { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}
