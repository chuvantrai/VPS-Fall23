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
            var imageSource = cameraView.GetSnapShot();
            Stream imageSourceStream = await ((StreamImageSource)imageSource).Stream.Invoke(CancellationToken.None);

            byte[] imageBytes;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await imageSourceStream.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
            }

            if (imageBytes != null)
            {
                var checkLicensePlate = new LicensePlateInfo
                {
                    Image = imageBytes,
                    CheckAt = DateTime.Now,
                    CheckBy = new Guid("D20939C1-7FA6-4DBB-B54A-3F6656AFA00E")
                };

                string apiResponse = await _viewModel.CheckLicensePLate(checkLicensePlate);

                await DisplayAlert(Constant.NOTIFICATION, apiResponse, Constant.CANCEL);
            }
            else
            {
                await DisplayAlert(Constant.ALERT, Constant.ALERT_ERROR, Constant.CANCEL);
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