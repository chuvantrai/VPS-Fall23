using InvoiceApi.ExternalData;
using System.Net.Mail;

namespace Service.ManagerVPS.ExternalClients
{
    public class BrokerApiClient
    {
        private readonly RestClient restClient;
        const string CreateDeletingPZJobUri = "api/parking-zone/create-deleting-parking-zone-job";
        const string RemoveDeletingPZJobUri = "api/parking-zone/{0}/remove-deleting-parking-zone-job";
        const string SendMailUri = "api/smtp/send-mail";

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
        public async Task<bool> SendMail(string[] receivers, string subject, string message, params Attachment[]? attachments)
        {
            var response = await this.restClient.PostAsJsonAsync(SendMailUri, new { receivers, subject, message, attachments = attachments });
            return response.IsSuccessStatusCode;
        }
    }
}