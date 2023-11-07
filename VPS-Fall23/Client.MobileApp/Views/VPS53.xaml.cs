using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Views;
using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using Client.MobileApp.Extensions;
using CommunityToolkit.Maui.Core.Platform;
using SkiaSharp;
using SkiaSharp.Views.Maui;

namespace Client.MobileApp.Views;

public partial class VPS53 : ContentPage
{

    private readonly VPS53ViewModel _viewModel;
    private SKRect frameRect;

    public VPS53()
    {
        frameRect = new SKRect(50, 50, 300, 200);
        VPS53ViewModel viewModel = new();
        InitializeComponent();
        BindingContext = viewModel;
        _viewModel = viewModel;
        cameraView.Loaded += CameraView_CamerasLoaded;
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.Transparent);

        float frameWidth = 400;
        float frameHeight = 235;

        float frameSize = Math.Min(e.Info.Width, e.Info.Height) * 0.5f;
        float frameX = (e.Info.Width - frameWidth) / 2;
        float frameY = (e.Info.Height - frameHeight) / 2;
        float frameRight = frameX + frameWidth;
        float frameBottom = frameY + frameHeight;

        using (var paint = new SKPaint())
        {
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = SKColors.Black;
            paint.StrokeWidth = 5;

            float dashLength = frameWidth * 0.125f;
            float dashGapthWidth = frameWidth - dashLength * 2;
            float dashGapthHeight = frameHeight - dashLength * 2;

            canvas.DrawLine(frameX, frameY, frameX + dashLength, frameY, paint);
            canvas.DrawLine(frameX + dashLength + dashGapthWidth, frameY, frameX + 2 * dashLength + dashGapthWidth, frameY, paint);
            canvas.DrawLine(frameX, frameBottom, frameX + dashLength, frameBottom, paint);
            canvas.DrawLine(frameX + dashLength + dashGapthWidth, frameBottom, frameX + 2 * dashLength + dashGapthWidth, frameBottom, paint);

            canvas.DrawLine(frameX, frameY, frameX, frameY + dashLength, paint);
            canvas.DrawLine(frameX, frameY + dashLength + dashGapthHeight, frameX, frameY + 2 * dashLength + dashGapthHeight, paint);
            canvas.DrawLine(frameRight, frameY, frameRight, frameY + dashLength, paint);
            canvas.DrawLine(frameRight, frameY + dashLength + dashGapthHeight, frameRight, frameBottom, paint);
        }
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

            using (var bitmap = SKBitmap.Decode(imageBytes))
            using (var croppedBitmap = new SKBitmap((int)frameRect.Width, (int)frameRect.Height))
            {
                using (var canvas = new SKCanvas(croppedBitmap))
                {
                    canvas.DrawBitmap(bitmap, new SKRect(0, 0, frameRect.Width, frameRect.Height), frameRect);
                }

                byte[] croppedImageBytes;
                using (var imageStream = new MemoryStream())
                {
                    croppedBitmap.Encode(imageStream, SKEncodedImageFormat.Png, 100);
                    croppedImageBytes = imageStream.ToArray();
                }

                if (imageBytes != null)
                {
                    var checkLicensePlate = new LicensePlateScan
                    {
                        Image = croppedImageBytes,
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