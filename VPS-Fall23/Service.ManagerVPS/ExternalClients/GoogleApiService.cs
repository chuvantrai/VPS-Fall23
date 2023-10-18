using Service.ManagerVPS.Constants.FileManagement;
using Google.Cloud.Vision.V1;

namespace Service.ManagerVPS.ExternalClients
{
    public class GoogleApiService
    {
        private readonly IConfiguration _config;

        public GoogleApiService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<string> GetLicensePlateFromImage(Google.Cloud.Vision.V1.Image image)
        {
            string licensePlate = String.Empty;
            string GoogleAppCredentials = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, Constant.GOOGLEAPPCREDENTIALS);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", GoogleAppCredentials);
            var client = ImageAnnotatorClient.Create();
            var response = await client.DetectTextAsync(image);

            if (response[0].Description != null)
            {
                licensePlate = string.Join("", response[0].Description.Split(new string[] { "\n" }, StringSplitOptions.None));
            }

            return licensePlate;
        }
    }
}
