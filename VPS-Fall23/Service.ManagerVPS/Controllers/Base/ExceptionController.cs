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
             var exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;



            object responseObject = new { code = 500, message = ResponseNotification.SERVER_ERROR };
            if (exceptionHandlerFeature.Error is VpsException vpsException)
            {
                responseObject = new { code = vpsException.Code, message = vpsException.Message };
            };


            switch (exceptionHandlerFeature.Error)
            {
                case ForbidenException:
                    {
                        return StatusCode(403, responseObject);
                    }
                case ClientException:
                    {
                        return BadRequest(responseObject);
                    }
                case UnauthorizeException:
                    {
                        return Unauthorized(responseObject);
                    }   
                case ServerException:
                default:
                    {
                        return StatusCode(500, responseObject);
                    }
            }
        }
    }
}