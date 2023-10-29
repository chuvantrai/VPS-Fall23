using Client.MobileApp.Models;
using System.Net.Http.Json;
using Client.MobileApp.Constants;
using System.Text.Json;

namespace Client.MobileApp.ViewModels
{
    public class VPS61ViewModel : ViewModelBase
    {
        private readonly HttpClient _client;

        public VPS61ViewModel()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5001")
                //BaseAddress = new Uri("http://localhost:5001")
            };
        }
        public async Task<string> CheckLicensePLate(LicensePlateInput checkLicensePlate)
        {

            HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS61, checkLicensePlate);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<ErrorResponse>(errorResponse);
                return $"{error.Code}{error.Message}";
            }
        }

        public async Task<string> CheckOutConfirm(LicensePlateInput checkLicensePlate)
        {
            HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS80, checkLicensePlate);

            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }
            else
            {
                string errorResponse = await response.Content.ReadAsStringAsync();
                var error = JsonSerializer.Deserialize<ErrorResponse>(errorResponse);
                return $"{error.Code}{error.Message}";
            }
        }
    }
}