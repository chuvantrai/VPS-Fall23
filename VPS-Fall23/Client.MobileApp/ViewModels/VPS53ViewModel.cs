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

        public async Task<string> CheckLicensePLate(LicensePlateScan checkLicensePlate)
        {

            HttpResponseMessage response = await _client.PostAsJsonAsync(Constant.API_PATH_VPS53, checkLicensePlate);

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

        public async Task<FileResult> OpenMediaPickerAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please a pick photo"
                });

                if (result.ContentType == "image/png" ||
                    result.ContentType == "image/jpeg" ||
                    result.ContentType == "image/jpg")
                    return result;
                else
                    await App.Current.MainPage.DisplayAlert("Error Type Image", "Please choose a new image", "Ok");

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<Stream> FileResultToStream(FileResult fileResult)
        {
            if (fileResult == null)
                return null;

            return await fileResult.OpenReadAsync();
        }

        public string ByteBase64ToString(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        public async Task<ImageFile> Upload(FileResult fileResult)
        {
            byte[] bytes;

            try
            {
                using (var ms = new MemoryStream())
                {
                    var stream = await FileResultToStream(fileResult);
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                }

                return new ImageFile
                {
                    byteBase64 = ByteBase64ToString(bytes),
                    ContentType = fileResult.ContentType,
                    FileName = fileResult.FileName
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
