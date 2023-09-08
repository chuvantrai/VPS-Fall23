namespace Service.FileManager.Extensions;

public interface IExtensionCRM
{
    string? CreateImage(IFormFile myFile);

    string? UpdateImage(IFormFile myFile, string oldFile);

    bool DeleteImage(string oldFile);
}