using Client.MobileApp.Models;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace Client.MobileApp.ViewModels
{
    public class VPS53ViewModel : ViewModelBase
    {
        private readonly HttpClient _client;

        public VPS53ViewModel()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5001")
            };
        }

        public async Task<string> CheckLicensePLate(LicensePlateInfo checkLicensePlate)
        {
            try
            {
                HttpResponseMessage response = await _client.PostAsJsonAsync("api/ParkingTransaction/CheckLicensePlate", checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    string errorResponse = await response.Content.ReadAsStringAsync();
                    var error = JsonSerializer.Deserialize<ErrorResponse>(errorResponse);
                    return $"Mã lỗi: {error.Code}, Thông báo: {error.Message}";
                }
                else
                {
                    return $"Yêu cầu không thành công, mã trạng thái: {response.StatusCode}";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
    }
}
