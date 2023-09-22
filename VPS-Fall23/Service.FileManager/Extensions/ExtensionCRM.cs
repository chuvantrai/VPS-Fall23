namespace Service.FileManager.Extensions;

public class ExtensionCRM : IExtensionCRM
{
    public string? CreateImage(IFormFile myFile)
    {
        try
        {
            if (myFile == null || myFile.Length == 0)
            {
                throw new Exception("File not found or empty.");
            }

            //add img
            var newFileName = Guid.NewGuid();
            var extension = Path.GetExtension(myFile.FileName);
            var fileName = newFileName + extension;

            const string folderPath = "image";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(),
                "wwwroot", folderPath, fileName);
            using var file = new FileStream(filePath, FileMode.Create);
            myFile.CopyTo(file);
            return fileName;
        }
        catch
        {
            return null;
        }
    }

    public string? UpdateImage(IFormFile myFile, string oldFile)
    {
        try
        {
            var fileName = CreateImage(myFile);
            // delete img 
            if (!string.IsNullOrEmpty(oldFile) && fileName != null)
            {
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "image", oldFile);
                var fileDelete = new FileInfo(imgPath);
                if (fileDelete.Length > 0)
                {
                    File.Delete(imgPath);
                    fileDelete.Delete();
                }

                return fileName;
            }

            return null;
        }
        catch
        {
            return null;
        }
    }

    public bool DeleteImage(string oldFile)
    {
        try
        {
            // delete img 
            if (!string.IsNullOrEmpty(oldFile))
            {
                var imgPath = Path.Combine(Directory.GetCurrentDirectory(),
                    "wwwroot", "image", oldFile);
                var fileDelete = new FileInfo(imgPath);
                if (fileDelete.Length > 0)
                {
                    File.Delete(imgPath);
                    fileDelete.Delete();
                    return true;
                }
            }

            return false;
        }
        catch
        {
            return false;
        }
    }
}