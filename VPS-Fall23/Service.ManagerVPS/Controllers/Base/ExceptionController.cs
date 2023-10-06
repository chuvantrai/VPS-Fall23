using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Service.ManagerVPS.Constants.Notifications;
using Service.ManagerVPS.DTO.Exceptions;

namespace Service.ManagerVPS.Controllers.Base
{
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    [ApiController]
    public class ExceptionController : Controller
    {
        [Route("/error")]
        public IActionResult HandleErrorDevelopment()
        {
            IExceptionHandlerFeature exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;
            switch (exceptionHandlerFeature.Error)
            {
                case ForbidenException forbidenException:
                    {
                        return StatusCode(403, forbidenException.Message);
                    }
                case ClientException clientException:
                    {
                        return BadRequest(clientException.Message);
                    }
                case UnAuthorizeException unAuthorizeException:
                    {
                        return Unauthorized(unAuthorizeException.Message);
                    }
                default:
                    {
                        return StatusCode(500, ResponseNotification.SERVER_ERROR);
                    }
            }
        }
    }
}
