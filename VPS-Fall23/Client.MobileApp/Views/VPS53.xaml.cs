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
    float frameWidth = 400;
    float frameHeight = 235;
    public VPS53()
    {
        VPS53ViewModel viewModel = new();
        Load();
        BindingContext = viewModel;
        _viewModel = viewModel;

    }

    public async void Load()
    {
        InitializeComponent();
        cameraView.Loaded += CameraView_CamerasLoaded;
        canvasView.PaintSurface += OnPaintSurface;
    }

    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var surface = e.Surface;
        var canvas = surface.Canvas;

        canvas.Clear(SKColors.Transparent);

        float frameX = (e.Info.Width - frameWidth) / 2;
        float frameY = (e.Info.Height - frameHeight) / 2;
        float frameRight = frameX + frameWidth;
        float frameBottom = frameY + frameHeight;

        using (var paint = new SKPaint())
        {
            paint.Style = SKPaintStyle.Stroke;
            paint.Color = SKColors.Black;
            paint.StrokeWidth = 5;

            float dashLength = 50f;
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

            frameRect = new SKRect(frameX, frameY, frameX + frameWidth, frameY + frameHeight);
        }
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        if (cameraView.Cameras.Count > 0)
        {
            cameraView.Camera = cameraView.Cameras.First();

            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await cameraView.StartCameraAsync();
            });
        }
    }

    private void FrameSwitch_Toggled(object sender, ToggledEventArgs e)
    {
        if (e.Value)
        {
            frameWidth = 590;
            frameHeight = 180;
        }
        else
        {
            frameWidth = 400;
            frameHeight = 235;
        }
        canvasView.InvalidateSurface();
        canvasView.PaintSurface += OnPaintSurface;
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
            using (var image = SKImage.FromBitmap(bitmap))
            {
                var frameImage = image.Subset(SKRectI.Create(300, 400, 235, 400));

                using (var frameBitmap = SKBitmap.FromImage(frameImage))
                {
                    byte[] frameImageBytes;

                    using (var imageStream = new MemoryStream())
                    {
                        frameBitmap.Encode(imageStream, SKEncodedImageFormat.Png, 100);
                        frameImageBytes = imageStream.ToArray();
                    }

                    if (imageBytes != null)
                    {
                        var checkLicensePlate = new LicensePlateScan
                        {
                            Image = frameImageBytes,
                            CheckAt = DateTime.Now,
                            CheckBy = Constant.USER
                        };

                        string response_1 = await _viewModel.CheckLicensePLateScan(checkLicensePlate);

                        if (response_1 == Constant.CHECKOUT_CONFIRM || response_1.Contains(Constant.OVERTIME_CONFIRM))
                        {
                            var answer = await DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                            if (answer.ToString() == Constant.ACCEPT)
                            {
                                string response_2 = await _viewModel.CheckOutScanConfirm(checkLicensePlate);

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
                Load();
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

                string response_1 = await _viewModel.CheckLicensePLateInput(checkLicensePlate);

                if (response_1 == Constant.CHECKOUT_CONFIRM)
                {
                    MainThread.BeginInvokeOnMainThread(async () =>
                    {
                        var answer = await Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                        if (answer.ToString() == Constant.ACCEPT)
                        {
                            string response_2 = await _viewModel.CheckOutInputConfirm(checkLicensePlate);

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