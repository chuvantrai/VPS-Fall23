namespace Client.MobileApp.Extensions
{
    public static class Logic
    {
        public static async Task<FileResult> OpenMediaPickerAsync()
        {
            try
            {
                var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Please a pick photo"
                });

                if (result.ContentType == "image/png" ||
                    result.ContentType == "image/jpeg" ||
                    result.ContentType == "image/jpg")
                    return result;
                else
                    await App.Current.MainPage.DisplayAlert("Error Type Image", "Please choose a new image", "Ok");

                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<byte[]> ConvertFileResultToByteArray(FileResult fileResult)
        {
            try
            {
                if (fileResult == null)
                {
                    return null;
                }

                using (var stream = await fileResult.OpenReadAsync())
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await stream.CopyToAsync(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static async Task<byte[]> ConvertStreamToByteArray(Stream stream)
        {
            try
            {
                byte[] imageBytes;
                using MemoryStream memoryStream = new();
                await stream.CopyToAsync(memoryStream);
                imageBytes = memoryStream.ToArray();
                return imageBytes;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
