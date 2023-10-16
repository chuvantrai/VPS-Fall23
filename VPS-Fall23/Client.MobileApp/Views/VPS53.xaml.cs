using Client.MobileApp.ViewModels;
using Google.Cloud.Vision.V1;
using Image = Google.Cloud.Vision.V1.Image;
using CommunityToolkit.Maui.Views;
using Client.MobileApp.Constants;
using Client.MobileApp.Extensions;
using Client.MobileApp.Models;

namespace Client.MobileApp.Views;

public partial class VPS53 : ContentPage
{

    private readonly VPS53ViewModel _viewModel;
    public VPS53()
    {
        VPS53ViewModel viewModel = new();
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
    }

    private void cameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StopCameraAsync();
                await cameraView.StartCameraAsync();
            });
        }
    }

    private async void CameraButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string licensePlate = String.Empty;
            string path = String.Empty;

            string localFilePath = Path.Combine(FileSystem.Current.AppDataDirectory, Constant.ImageName);
            await cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.JPEG, localFilePath);
#if ANDROID
            await Logic.CopyFileToAppDataDirectory(Constant.GoogleAppCredentials);
            path = Path.Combine(FileSystem.Current.AppDataDirectory, Constant.GoogleAppCredentials);
#elif WINDOWS
            path = Path.Combine(System.IO.Directory.GetCurrentDirectory(), Constant.GoogleAppCredentials);
#endif
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            var client = ImageAnnotatorClient.Create();
            var image = Image.FromFile(localFilePath);

            var response = client.DetectText(image);

            if (response[0].Description != null)
            {
                licensePlate += response[0].Description;
                var checkLicensePlate = new CheckLicensePlate
                {
                    LicensePlate = licensePlate,
                    CheckAt = DateTime.Now,
                    CheckBy = new Guid()
                };

                string apiResponse = await _viewModel.CheckLicensePLate(checkLicensePlate);

                await DisplayAlert("NOTIFICATION", apiResponse, "Cancel");
            }
            else
            {
                await DisplayAlert("ALERT", "PLEASE TAKE THE LICENSE PLATE IN TO AREA !!!", "Cancel");
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {

    }

    private void LincenseButton_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new LicenseInputPopup());
    }
}