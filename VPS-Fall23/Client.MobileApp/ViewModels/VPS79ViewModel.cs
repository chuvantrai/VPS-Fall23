using Client.MobileApp.Models;
using System.Net.Http.Json;
using Client.MobileApp.Constants;
using System.Text.Json;

namespace Client.MobileApp.ViewModels
{
    public class VPS79ViewModel : ViewModelBase
    {
        private readonly HttpClient _client;
        bool isBusy = false;
        int cameraIndex = 2;
        int loadingIndex = 0;
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
        public VPS79ViewModel()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("http://10.0.2.2:5001")
                //BaseAddress = new Uri("http://localhost:5001")
            };
        }

        public async Task<string> CheckAccount(LoginRequest loginRequest)
        {
            return await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                IsBusy = true;
                CameraIndex = -1;
                LoadingIndex = 1;
                HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS79, loginRequest);

                if (response.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    Constant.USER = await response.Content.ReadFromJsonAsync<Guid>();
                    return Constant.LOGIN_SUCCESS;
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