namespace Service.WorkerVPS.ExternalClients
{
    internal class VpsClient
    {
        readonly RestClient restClient;
        static string DeleteParkingZoneUri = "api/ParkingZone/DeleteParkingZone/{0}";
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

    }
}
