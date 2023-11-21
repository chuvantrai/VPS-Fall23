using Client.MobileApp.ViewModels;
using CommunityToolkit.Maui.Views;
using Client.MobileApp.Constants;
using Client.MobileApp.Models;
using CommunityToolkit.Maui.Core.Platform;
using SkiaSharp;
using SkiaSharp.Views.Maui;
using Microsoft.Maui.Graphics;
using System.IO;


namespace Client.MobileApp.Views;

public partial class VPS53 : ContentPage
{

    private readonly VPS53ViewModel _viewModel;
    private SKRect plateRect;
    const float PLATE_LONG_H_W = 0.3f;
    const float PLATE_SHORT_H_W = 0.74f;
    const float SCREEN_COVERAGE = 0.8f;
    bool isShortPlate = true;

    public VPS53()
    {
        InitializeComponent();
        VPS53ViewModel viewModel = new();
        BindingContext = viewModel;
        _viewModel = viewModel;
        FrameSwitch.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.2;
        ChangePlateTypeButton.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.2;
        LogoImage.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.3;
        LoadCanvasSurface();
        cameraView.Loaded += CameraView_CamerasLoaded;
    }

    public void Load()
    {
        InitializeComponent();
    }
    Task LoadCanvasSurface()
    {
        return Task.Run(() =>
        {
            canvasView.InvalidateSurface();
            canvasView.PaintSurface += OnPaintSurface;
        });
    }
    private void OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
    {
        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        var frameWidthFollowScreen = e.Info.Width * SCREEN_COVERAGE;
        var frameHeightFollowScreen = frameWidthFollowScreen * (isShortPlate ? PLATE_SHORT_H_W : PLATE_LONG_H_W);
        float frameX = (e.Info.Width - frameWidthFollowScreen) / 2;
        float frameY = (e.Info.Height - frameHeightFollowScreen) / 2;
        float frameRight = frameX + frameWidthFollowScreen;
        float frameBottom = frameY + frameHeightFollowScreen;
        SKRect allScreenRect = new SKRect(0, 0, e.Info.Width, e.Info.Height);
        plateRect = new SKRect(frameX, frameY, frameRight, frameBottom);
        using var paint = new SKPaint();
        using SKRegion allScreenRegion = new SKRegion(SKRectI.Round(allScreenRect));
        using SKRegion plateRegion = new SKRegion(SKRectI.Round(plateRect));
        paint.Style = SKPaintStyle.Fill;
        paint.Color = SKColors.Black.WithAlpha((byte)(0xFF * (1 - 0.5)));
        paint.StrokeWidth = 5;
        allScreenRegion.Op(plateRegion, SKRegionOperation.Difference);
        canvas.DrawRegion(allScreenRegion, paint);
    }

    private void CameraView_CamerasLoaded(object sender, EventArgs e)
    {
        cameraView.Camera = cameraView.Cameras.First();

        MainThread.BeginInvokeOnMainThread(async () =>
        {
            await cameraView.StopCameraAsync();
            await cameraView.StartCameraAsync();
        });
    }

    SKRectI GetImageSubsetArea(SKImage image)
    {
        var remainY = LicenseInputFrame.Height + ButtonFrame.Height - Logo.Height;
        float left = plateRect.Left;
        float top = plateRect.Top + (float)remainY;
        float right = plateRect.Right;
        float bottom = plateRect.Bottom + (float)remainY;
        SKRect scropRect = new SKRect(left, top, right, bottom);
        return SKRectI.Floor(scropRect);
    }

    private async void CameraButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string licensePlate = String.Empty;
            string path = String.Empty;

            var imageSource = cameraView.GetSnapShot(Camera.MAUI.ImageFormat.PNG);
            StreamImageSource streamImageSource = (StreamImageSource)imageSource;
            CancellationToken cancellationToken = CancellationToken.None;
            Task<Stream> task = streamImageSource.Stream(cancellationToken);
            Stream stream = task.Result;
            stream.Seek(0, SeekOrigin.Begin);
            var newImage = SKImage.FromEncodedData(stream);
            var imageSubset = GetImageSubsetArea(newImage);
            var licensePlateImage = newImage.Subset(imageSubset);
            using MemoryStream imageSubsetStream = new MemoryStream();
            licensePlateImage.Encode(SKEncodedImageFormat.Png, 100)
              .SaveTo(imageSubsetStream);

            var checkLicensePlate = new LicensePlateScan
            {
                Image = imageSubsetStream.ToArray(),
                CheckAt = DateTime.Now,
                CheckBy = Constant.USER
            };
            string autoCheckinResponse = await _viewModel.CheckLicensePLateScan(checkLicensePlate);
            var checkoutScanConfirm = new Task<string>(() => _viewModel.CheckOutScanConfirm(checkLicensePlate).Result);
            await HandleResponse(autoCheckinResponse, checkoutScanConfirm);
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(Constant.ALERT, ex.Message, Constant.CANCEL);
            });
        }
    }

    async Task HandleResponse(string response, Task<string> checkOutScanConfirmTask = null)
    {
        switch (response)
        {
            case Constant.CHECKOUT_CONFIRM:
                {
                    var confirmAnswer = await DisplayAlert(Constant.NOTIFICATION, response, Constant.ACCEPT, Constant.CANCEL);
                    if (confirmAnswer.ToString() == Constant.ACCEPT)
                        await HandleResponse(Constant.ACCEPT);
                    break;
                }
            case Constant.ACCEPT:
                {
                    string checkinByConfirmResponse = await checkOutScanConfirmTask;
                    await HandleResponse(checkinByConfirmResponse);
                    break;
                }
            default:
                {
                    if (response.Contains(Constant.OVERTIME_CONFIRM))
                    {
                        await HandleResponse(Constant.CHECKOUT_CONFIRM);
                        break;
                    }
                    await DisplayAlert(Constant.NOTIFICATION, response, Constant.CANCEL);
                    break;
                }
        }
    }

    private async void OnTapGestureRecognizerTapped(object sender, TappedEventArgs e)
    {
        await LicensePlateEntry.HideKeyboardAsync(CancellationToken.None);

        LicensePlateEntry.Unfocus();
    }

    private async void OkButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string licensePlate = new string(LicensePlateEntry.Text.Where(c => Char.IsLetterOrDigit(c)).ToArray());

            if (!String.IsNullOrEmpty(licensePlate))
            {
                var checkLicensePlate = new LicensePlateInput
                {
                    LicensePlate = licensePlate.ToUpper(),
                    CheckAt = DateTime.Now,
                    CheckBy = Constant.USER
                };

                string autoCheckinResponse = await _viewModel.CheckLicensePLateInput(checkLicensePlate);
                var checkoutInputConfirm = new Task<string>(() => _viewModel.CheckOutInputConfirm(checkLicensePlate).Result);
                await HandleResponse(autoCheckinResponse, checkoutInputConfirm);
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Application.Current.MainPage.DisplayAlert(Constant.ALERT, Constant.INPUT_FAILED, Constant.CANCEL);
                });
            }
        }
        catch (Exception ex)
        {
            MainThread.BeginInvokeOnMainThread(async () =>
            {
                Application.Current.MainPage.DisplayAlert(Constant.ALERT, ex.Message, Constant.CANCEL);
            });
        }
    }

    private void ChangePlateType(object sender, EventArgs e)
    {
        isShortPlate = !isShortPlate;
        LoadCanvasSurface();
    }

    private async void LogOutButton_Clicked(object sender, EventArgs e)
    {
        SecureStorage.Remove("UserToken");
        await Navigation.PushAsync(new VPS79());
    }
}