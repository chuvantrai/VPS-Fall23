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
}