using InvoiceApi.ExternalData;
using Service.ManagerVPS.Models;
using System.Net.Mail;

namespace Service.ManagerVPS.ExternalClients
{
    public class BrokerApiClient
    {
        private readonly RestClient restClient;
        const string CreateDeletingPZJobUri = "api/parking-zone/create-deleting-parking-zone-job";
        const string RemoveDeletingPZJobUri = "api/parking-zone/{0}/remove-deleting-parking-zone-job";
        const string RemoveCancelBookingJobUri = "api/parking-transaction/create-auto-cancel-job";
        const string CreateCancelBookingJobUri = "api/parking-transaction/{0}/remove-auto-cancel-job";
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
        public async Task<bool> CreateCancelBookingJob(Guid parkingTransactionId, DateTime cancelAt)
        {
            var response = await this.restClient.PostAsJsonAsync(CreateCancelBookingJobUri,
                new { parkingTransactionId, cancelAt });
            return response.IsSuccessStatusCode;

        }
        public async Task<bool> RemoveCancelBookingJob(Guid parkingTransactionId)
        {
            string uri = string.Format(RemoveCancelBookingJobUri, parkingTransactionId);
            var response = await this.restClient.DeleteAsync(uri);
            return response.IsSuccessStatusCode;
        }
    }
}