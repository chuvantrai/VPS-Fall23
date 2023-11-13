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
        FrameSwitch.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.1;
        ChangePlateTypeButton.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.1;
        LoadCanvasSurface();

        cameraView.Loaded += CameraView_CamerasLoaded;
    }

    public void Load()
    {
        InitializeComponent();
        FrameSwitch.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.1;
        ChangePlateTypeButton.WidthRequest = DeviceDisplay.MainDisplayInfo.Width * 0.1;
        cameraView.Loaded += CameraView_CamerasLoaded;
        LoadCanvasSurface();
    }
    Task LoadCanvasSurface()
    {
        return Task.Run(() =>
        {
            canvasView.InvalidateSurface();
            canvasView.PaintSurface += OnPaintSurface;
            canvasView.InvalidateSurface();
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
        if (cameraView.Cameras.Count > 0)
        {

            cameraView.Camera = cameraView.Cameras[0];
            var maxResolution = cameraView.Camera.AvailableResolutions[0];
            foreach (var resolution in cameraView.Camera.AvailableResolutions)
            {
                if (resolution.Width * resolution.Height > maxResolution.Width * maxResolution.Height)
                {
                    maxResolution = resolution;
                }

            }

            double widthRatio = maxResolution.Width / DeviceDisplay.MainDisplayInfo.Width;
            double heightRatio = maxResolution.Height / DeviceDisplay.MainDisplayInfo.Height;

            double minRatio = Math.Min(widthRatio, heightRatio);

            cameraView.WidthRequest = maxResolution.Width / minRatio;
            cameraView.HeightRequest = maxResolution.Height / minRatio;

            var startCameraResult = cameraView.StartCameraAsync().Result;
            if (startCameraResult != 0)
            {
                Application.Current.MainPage.DisplayAlert(Constant.ALERT, startCameraResult.ToString(), Constant.CANCEL);
            }


        }
    }
    SKRectI GetImageSubsetArea(SKImage image)
    {
        float remainX = (float)(image.Width - cameraView.Width) / 2;
        float remainY = (float)(image.Height - cameraView.Height) / 2;
        float left = plateRect.Left + remainX;
        float top = plateRect.Top * (float)(image.Height / cameraView.Height);
        float right = plateRect.Right + remainX;
        float bottom = plateRect.Bottom * (float)(image.Height / cameraView.Height);
        SKRect scropRect = new SKRect(left, top, right, bottom);
        return SKRectI.Floor(scropRect);
    }
    private async void CameraButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string licensePlate = String.Empty;
            string path = String.Empty;

            var imageSource = await cameraView.TakePhotoAsync();
            imageSource.Seek(0, SeekOrigin.Begin);
            using var image = SKImage.FromEncodedData(imageSource);
            var imageSubset = GetImageSubsetArea(image);

            using var licensePlateImage = image.Subset(imageSubset);
            using MemoryStream imageSubsetStream = new MemoryStream();
            licensePlateImage.Encode(SKEncodedImageFormat.Png, 100).SaveTo(imageSubsetStream);
            File.WriteAllBytes("/storage/emulated/0/DCIM/Screenshots/img_cut.jpg", imageSubsetStream.ToArray());
            var checkLicensePlate = new LicensePlateScan
            {
                Image = imageSubsetStream.ToArray(),
                CheckAt = DateTime.Now,
                CheckBy = Constant.USER
            };
            string autoCheckinResponse = await _viewModel.CheckLicensePLateScan(checkLicensePlate);
            var checkoutScanConfirm = new Task<string>(() => _viewModel.CheckOutScanConfirm(checkLicensePlate).Result);
            await HandleResponse(autoCheckinResponse, checkoutScanConfirm);
            Load();
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
                        var answer = Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_1, Constant.ACCEPT, Constant.CANCEL);

                        if (answer.ToString() == Constant.ACCEPT)
                        {
                            string response_2 = await _viewModel.CheckOutInputConfirm(checkLicensePlate);

                            Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_2, Constant.CANCEL);
                        }
                        else
                        {
                            Application.Current.MainPage.DisplayAlert(Constant.NOTIFICATION, response_1, Constant.CANCEL);
                        }
                    });
                }
            }
            else
            {
                MainThread.BeginInvokeOnMainThread(async () =>
                {
                    Application.Current.MainPage.DisplayAlert(Constant.ALERT, Constant.ALERT_ERROR, Constant.CANCEL);
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
}