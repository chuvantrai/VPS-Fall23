namespace Service.BrokerApi.Models.ParkingZoneJob
{
    public class AutoDeleteDto
    {
        public Guid ParkingZoneAbsentId { get; set; }   
        public Guid ParkingZoneId { get; set; }
        public DateTime DeleteAt { get; set; }
    }
}
