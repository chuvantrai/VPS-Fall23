using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Views;
using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using Client.MobileApp.Extensions;

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

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
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

            var imageBytes = await Logic.ConvertStreamToByteArray(imageSourceStream);
            
            if (imageBytes != null)
            {
                var checkLicensePlate = new LicensePlateScan
                {
                    Image = imageBytes,
                    CheckAt = DateTime.Now,
                    CheckBy = Constant.USER
                };

                string response_1 = await _viewModel.CheckLicensePLate(checkLicensePlate);

                if (response_1 == Constant.CHECKOUT_CONFIRM || response_1.Contains(Constant.OVERTIME_CONFIRM))
                {
                    var answer = await DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                    if (answer.ToString() == Constant.ACCEPT)
                    {
                        string response_2 = await _viewModel.CheckOutConfirm(checkLicensePlate);

                        await DisplayAlert(Constant.NOTIFICATION, response_2, Constant.CANCEL);
                    }
                }
                else
                {
                    await DisplayAlert(Constant.NOTIFICATION, response_1, Constant.CANCEL);
                }

            }
            else
            {
                await DisplayAlert(Constant.ALERT, Constant.ALERT_ERROR, Constant.CANCEL);
            }
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(Constant.ALERT, ex.Message, Constant.CANCEL);
            });

        }
    }

    private async void ImageButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            var imageBytes = await Logic.ConvertFileResultToByteArray(await Logic.OpenMediaPickerAsync());
            if (imageBytes != null)
            {
                var checkLicensePlate = new LicensePlateScan
                {
                    Image = imageBytes,
                    CheckAt = DateTime.Now,
                    CheckBy = Constant.USER
                };

                string response_1 = await _viewModel.CheckLicensePLate(checkLicensePlate);

                if (response_1 == Constant.CHECKOUT_CONFIRM)
                {
                    var answer = await DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                    if(answer.ToString() == Constant.ACCEPT)
                    {
                        string response_2 = await _viewModel.CheckOutConfirm(checkLicensePlate);

                        await DisplayAlert(Constant.NOTIFICATION, response_2, Constant.CANCEL);
                    }
                }
                else
                {
                    await DisplayAlert(Constant.NOTIFICATION, response_1, Constant.CANCEL);
                }

            }
            else
            {
                await DisplayAlert(Constant.ALERT, Constant.ALERT_ERROR, Constant.CANCEL);
            }
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(Constant.ALERT, ex.Message, Constant.CANCEL);
            });

        }
    }

    private void LincenseButton_Clicked(object sender, EventArgs e)
    {
        this.ShowPopup(new VPS61());
    }
}