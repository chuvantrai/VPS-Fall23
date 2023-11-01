using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Views;
using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using Client.MobileApp.Extensions;
using CommunityToolkit.Maui.Core.Platform;

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
        cameraView.Loaded += CameraView_CamerasLoaded;
    }

    private void LoadCamera()
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

            var imageSource = await cameraView.TakePhotoAsync();
            imageSource.Seek(0, SeekOrigin.Begin);
            var imageBytes = await Logic.ConvertStreamToByteArray(imageSource);

            if (imageBytes != null)
            {
                var checkLicensePlate = new LicensePlateScan
                {
                    Image = imageBytes,
                    CheckAt = DateTime.Now,
                    CheckBy = Constant.USER
                };

                await cameraView.StopCameraAsync();
                string response_1 = await _viewModel.CheckLicensePLateScan(checkLicensePlate);
                LoadCamera();

                if (response_1 == Constant.CHECKOUT_CONFIRM || response_1.Contains(Constant.OVERTIME_CONFIRM))
                {
                    var answer = await DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                    if (answer.ToString() == Constant.ACCEPT)
                    {
                        await cameraView.StopCameraAsync();
                        string response_2 = await _viewModel.CheckOutScanConfirm(checkLicensePlate);
                        LoadCamera();

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
            await imageSource.DisposeAsync();
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(Constant.ALERT, ex.Message, Constant.CANCEL);
            });

        }
    }

    private async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        await LicensePlateEntry.HideKeyboardAsync(CancellationToken.None);
        await AreaCodeEntry.HideKeyboardAsync(CancellationToken.None);

        LicensePlateEntry.Unfocus();
        AreaCodeEntry.Unfocus();
    }

    private async void OkButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string licensePlate = AreaCodeEntry.Text + "-" + LicensePlateEntry.Text;

            if (!String.IsNullOrEmpty(licensePlate))
            {
                var checkLicensePlate = new LicensePlateInput
                {
                    LicensePlate = licensePlate,
                    CheckAt = DateTime.Now,
                    CheckBy = Constant.USER
                };

                await cameraView.StopCameraAsync();
                string response_1 = await _viewModel.CheckLicensePLateInput(checkLicensePlate);
                LoadCamera();

                if (response_1 == Constant.CHECKOUT_CONFIRM)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var answer = await Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                        if (answer.ToString() == Constant.ACCEPT)
                        {
                            await cameraView.StopCameraAsync();
                            string response_2 = await _viewModel.CheckOutInputConfirm(checkLicensePlate);
                            LoadCamera();

                            await Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_2, Constant.CANCEL);
                        }
                        else
                        {
                            await Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_1, Constant.CANCEL);
                        }
                    });
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    await Application.Current.MainPage.DisplayAlert(Constant.ALERT, Constant.ALERT_ERROR, Constant.CANCEL);
                });
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
}