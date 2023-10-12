using Client.MobileApp.ViewModels;
using System.IO;
using Google.Cloud.Vision.V1;
using System;
using Image = Google.Cloud.Vision.V1.Image;
using Microsoft.Maui.Storage;
using Microsoft.Maui.Controls.PlatformConfiguration;

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

    private async void CameraButton_Clicked(object sender, EventArgs e)
    {
        try
        {
            string localFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), "imageTest.jpg");
            await cameraView.SaveSnapShot(Camera.MAUI.ImageFormat.JPEG, localFilePath);


                string imageText = "";
            string fileName = "googleKey.json";
            string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), fileName);

            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            var client = ImageAnnotatorClient.Create();
            var image = Image.FromFile("C:\\Users\\trank\\OneDrive\\Pictures\\263152245_861121767886264_3984037907517320615_n.jpg");

            var response = client.DetectText(image);

            foreach (var annotation in response)
            {
                if (annotation.Description != null)
                {
                    imageText += annotation.Description;
                }
            }

            string ing = imageText.Trim();

        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    private void ImageButton_Clicked(object sender, EventArgs e)
    {

    }
}