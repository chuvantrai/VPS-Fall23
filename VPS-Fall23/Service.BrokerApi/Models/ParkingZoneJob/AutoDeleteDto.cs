namespace Service.BrokerApi.Models.ParkingZoneJob
{
    public class AutoDeleteDto
    {
        public Guid ParkingZoneId { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}
