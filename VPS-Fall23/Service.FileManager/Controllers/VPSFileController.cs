using Microsoft.AspNetCore.Mvc;
using Service.FileManager.Extensions;
using Service.FileManager.Constants.Notifications;

namespace Service.FileManager.Controllers;

[ApiController]
[Route("[controller]")]
public class VPSFileController : Controller
{
    private readonly IExtensionFile _extension;
    private const long SizeLimitImage = 10;

    public VPSFileController(IExtensionFile extension)
    {
        _extension = extension;
    }

    [HttpPost("CreateImage")]
    public IActionResult CreateImage(IFormFile imageFile)
    {
        try
        {
            var fileSize = imageFile.Length / (1024f * 1000f);
            if (fileSize < SizeLimitImage)
            {
                var nameImage = _extension.CreateImage(imageFile);
                if(nameImage != null)
                {
                    return Ok(new
                    {
                        success = true,
                        status = 200,
                        data = nameImage
                    });
                }
            }
            return NotFound($"{ResponseNotification.OVER_SIZE} ({SizeLimitImage}MB)");
        }
        catch
        {
            return NotFound(ResponseNotification.SERVER_ERROR);
        }
    }
    [HttpPost("DeleteImage")]
    public IActionResult DeleteImage(string fileName)
    {
        try
        {
            return Ok("Done");
        }
        catch
        {
            return NotFound(ResponseNotification.SERVER_ERROR);
        }
    }
}