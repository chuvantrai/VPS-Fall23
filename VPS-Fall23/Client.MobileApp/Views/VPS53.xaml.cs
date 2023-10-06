using Client.MobileApp.ViewModels;
using IronOcr;

namespace Client.MobileApp.Views;

public partial class VPS53 : ContentPage
{

    private readonly VPS53ViewModel _viewModel;
    public VPS53()
    {
        VPS53ViewModel viewModel = new VPS53ViewModel();
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

    private async void Button_Clicked(object sender, EventArgs e)
    {
        try
        {
            var photo = await cameraView.CaptureAsync();
            string localFilePath = Path.Combine(FileSystem.AppDataDirectory, "imagesTest");

            using var sourceStream = await photo.OpenReadAsync();
            using var memoryStream = new MemoryStream();
            sourceStream.CopyTo(memoryStream);

            sourceStream.Position = 0;
            memoryStream.Position = 0;

#if WINDOWS
            await System.IO.File.WriteAllBytesAsync(@"D:\FULearning\Fall2023\C# Code\repos\VPS-Fall23\Client.MobileApp\Images\Test.png", memoryStream.ToArray());
#elif ANDROID
#elif IOS || MACCATALYST
#endif

            //if (!Directory.Exists(localFilePath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}
            //using (var destinationStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            //{
            //    await photo.CopyToAsync(destinationStream, ScreenshotFormat.Png, 100);
            //}
            string imageText = "";
            imageText = new IronTesseract().Read("D:\\FULearning\\Fall2023\\C# Code\\repos\\VPS-Fall23\\Client.MobileApp\\Images\\Test.png").Text;

            string ing = imageText.Trim();

        }catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }
}