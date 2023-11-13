using InvoiceApi.ExternalData;

namespace Service.ManagerVPS.ExternalClients
{
    public class BrokerApiClient
    {
        private readonly RestClient restClient;
        const string CreateDeletingPZJobUri = "create-deleting-parking-zone-job";
        const string RemoveDeletingPZJobUri = "{0}/remove-deleting-parking-zone-job";

        public BrokerApiClient(string baseUrl)
        {
            restClient = new RestClient(baseUrl);
        }

        public async Task<bool> CreateDeletingPZJob(Guid parkingZoneAbsentId, Guid parkingZoneId,
            DateTime deleteAt)
        {
            var response = await this.restClient.PostAsJsonAsync(CreateDeletingPZJobUri,
                new { parkingZoneAbsentId, parkingZoneId, deleteAt });
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveDeletingPZJob(Guid parkingZoneAbsentId)
        {
            string uri = string.Format(RemoveDeletingPZJobUri, parkingZoneAbsentId);
            var response = await this.restClient.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
        }
    }
}