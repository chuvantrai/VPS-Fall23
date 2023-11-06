namespace Service.WorkerVPS.Models.ParkingZoneJob
{
    internal class AutoDeleteParkingZoneDto
    {
        public Guid ParkingZoneId { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}
