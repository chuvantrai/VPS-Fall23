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
                var apiTask = _client.PostAsJsonAsync(Constant.API_PATH_VPS79, loginRequest);
                var completedTask = await Task.WhenAny(apiTask, Task.Delay(2000));
                if (completedTask == apiTask && apiTask.Result.IsSuccessStatusCode)
                {
                    IsBusy = false;
                    CameraIndex = 1;
                    LoadingIndex = -1;
                    Constant.USER = await apiTask.Result.Content.ReadFromJsonAsync<Guid>();
                    return Constant.LOGIN_SUCCESS;
                }
                else
                {
                    IsBusy = false;
                    CameraIndex = 1;
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
    }
}