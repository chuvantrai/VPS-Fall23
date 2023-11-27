namespace Service.BrokerApi.Models.ParkingZoneJob
{
    public class AutoCancelBookingDto
    {

        public Guid ParkingTransactionId { get; set; }
        public DateTime CancelAt { get; set; }

    }
}
