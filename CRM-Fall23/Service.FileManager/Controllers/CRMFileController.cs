using Microsoft.AspNetCore.Mvc;
using Service.FileManager.Constants.Notifications;
using Service.FileManager.Extensions;

namespace Service.FileManager.Controllers;

[ApiController]
[Route("[controller]")]
public class CRMFileController : Controller
{
    private readonly IExtensionCRM _extension;
    private const long SizeLimitImage = 10;

    public CRMFileController(IExtensionCRM extension)
    {
        _extension = extension;
    }

    [HttpPost("CreateImage")]
    public IActionResult CreateImage(IFormFile imageFile)
    {
        try
        {
            var fileSize = imageFile.Length / (1024 * 1024 * 50);
            if (fileSize < SizeLimitImage)
            {
                var nameImage = _extension.CreateImage(imageFile);
                return Ok(new
                {
                    success = true,
                    status = 200,
                    data = nameImage
                });
            }

            return NotFound($"{ResponseNotification.OVER_SIZE} ({SizeLimitImage}MB)");
        }
        catch
        {
            return NotFound(ResponseNotification.SERVER_ERROR);
        }
    }
}