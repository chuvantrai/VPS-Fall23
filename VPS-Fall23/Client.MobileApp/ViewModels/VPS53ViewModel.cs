using Client.MobileApp.Models;
using System.Net.Http.Json;
using Client.MobileApp.Constants;
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
                //BaseAddress = new Uri("http://localhost:5001")
            };
        }
        public async Task<string> CheckLicensePLateScan(LicensePlateScan checkLicensePlate)
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                CameraIndex = -1;
                LoadingIndex = 1;

                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS53, checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;

                    var ObjectResponse = await response.Content.ReadFromJsonAsync<LicensePlateScanResponse>();

                    string[] licensePlate = ObjectResponse.LicensePlate.Split('-');

                    AreaEntry = licensePlate[0];
                    LicensePlateEntry = licensePlate[1];

                    return ObjectResponse.Notification;
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;

                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return $"{errorResponse.Message}";
                }
            });
        }

        public async Task<string> CheckOutScanConfirm(LicensePlateScan checkLicensePlate)
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                CameraIndex = -1;
                LoadingIndex = 1;

                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS80_1, checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return $"{errorResponse.Message}";
                }
            });
        }

        public async Task<string> CheckLicensePLateInput(LicensePlateInput checkLicensePlate)
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                CameraIndex = -1;
                LoadingIndex = 1;

                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS61, checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return $"{errorResponse.Message}";
                }
            });
        }

        public async Task<string> CheckOutInputConfirm(LicensePlateInput checkLicensePlate)
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                CameraIndex = -1;
                LoadingIndex = 1;
                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS80_2, checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return $"{errorResponse.Message}";
                }
            });
        }
    }
}
