using Client.MobileApp.Models;
using System.Net.Http.Json;
using Client.MobileApp.Constants;
using System.Text.Json;

namespace Client.MobileApp.ViewModels
{
    public class VPS53ViewModel : ViewModelBase
    {
        private readonly HttpClient _client;
        bool isBusy = false;
        int cameraIndex = 1;
        int loadingIndex = -1;
        int layoutIndex = 2;
        string areaEntry = "";
        string licensePlateEntry = "";

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        public string AreaEntry
        {
            get { return areaEntry; }
            set { SetProperty(ref areaEntry, value); }
        }

        public string LicensePlateEntry
        {
            get { return licensePlateEntry; }
            set { SetProperty(ref licensePlateEntry, value); }
        }

        public int CameraIndex
        {
            get { return cameraIndex; }
            set { SetProperty(ref cameraIndex, value); }
        }
        public int LayoutIndex
        {
            get { return layoutIndex; }
            set { SetProperty(ref layoutIndex, value); }
        }


        public int LoadingIndex
        {
            get { return loadingIndex; }
            set { SetProperty(ref loadingIndex, value); }
        }

        string title = string.Empty;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }
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
                LayoutIndex = 1;
                LoadingIndex = 2;

                var apiTask = _client.PostAsJsonAsync(Constant.API_PATH_VPS53, checkLicensePlate);
                var completedTask = await Task.WhenAny(apiTask, Task.Delay(2000));
                if (completedTask == apiTask && apiTask.Result.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    LayoutIndex = 2;
                    LoadingIndex = -1;

                    var ObjectResponse = await apiTask.Result.Content.ReadFromJsonAsync<LicensePlateScanResponse>();

                    string[] licensePlate = ObjectResponse.LicensePlate.Split('-');

                    AreaEntry = licensePlate[0];
                    LicensePlateEntry = licensePlate[1];

                    return ObjectResponse.Notification;
                }
                else
                {
                    IsBusy = false;
                    LayoutIndex = 2;
                    LoadingIndex = -1;

                    if (!apiTask.Wait(0))
                    {
                        return "API call timed out.";
                    }

                    var errorResponse = await apiTask.Result.Content.ReadFromJsonAsync<ErrorResponse>();
                    return $"{errorResponse.Message}";
                }
            });
        }

        public async Task<string> CheckOutScanConfirm(LicensePlateScan checkLicensePlate)
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                CameraIndex = 0;
                LoadingIndex = 2;

                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS80_1, checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 2;
                    LoadingIndex = 0;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 2;
                    LoadingIndex = 0;
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
                CameraIndex = 0;
                LoadingIndex = 2;

                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS61, checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 2;
                    LoadingIndex = 0;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = 0;
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
                CameraIndex = 0;
                LoadingIndex = 2;
                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS80_2, checkLicensePlate);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 2;
                    LoadingIndex = -1;
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 2;
                    LoadingIndex = 0;
                    var errorResponse = await response.Content.ReadFromJsonAsync<ErrorResponse>();
                    return $"{errorResponse.Message}";
                }
            });
        }
    }
}
