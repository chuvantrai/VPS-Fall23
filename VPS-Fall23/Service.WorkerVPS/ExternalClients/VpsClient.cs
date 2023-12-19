namespace Service.WorkerVPS.ExternalClients
{
    internal class VpsClient
    {
        readonly RestClient restClient;
        static readonly string DeleteParkingZoneUri = "api/ParkingZone/DeleteParkingZone/{0}";
        static readonly string CancelBookingTransactionUri = "api/ParkingTransaction/Cancel/{0}";
        static readonly string JobSendNotificationPromoCodeToUser = "api/PromoCode/JobSendNotificationPromoCodeToUser";
        public VpsClient(string baseUrl)
        {
            restClient = new RestClient(baseUrl);
        }
        public async Task<bool> DeleteParkingZone(Guid id)
        {
            string uri = string.Format(DeleteParkingZoneUri, id);

            var response = await this.restClient.Delete(uri);
            return response;
        }
        public async Task<bool> CancelBookingTransaction(Guid bookingId)
        {
            string uri = string.Format(CancelBookingTransactionUri, bookingId);

            var response = await this.restClient.Delete(uri);
            return response;
        }
        public async Task SendNotificationPromoCodeToUser()
        {
            var uri = string.Format(JobSendNotificationPromoCodeToUser);

            var response = await this.restClient.Get<bool>(uri);
        }
    }
}
