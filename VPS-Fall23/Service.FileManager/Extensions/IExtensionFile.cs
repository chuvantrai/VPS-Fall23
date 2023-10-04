namespace Service.FileManager.Extensions;

public interface IExtensionFile
{
    string? CreateImage(IFormFile myFile);

    string? UpdateImage(IFormFile myFile, string oldFile);

    bool DeleteImage(string oldFile);
}