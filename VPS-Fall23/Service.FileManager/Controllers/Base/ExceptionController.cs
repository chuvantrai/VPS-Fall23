using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace VPS.MinIO.API.Controllers.Base
{
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ExceptionController : ControllerBase
    {

        public ExceptionController() { }
        [HttpGet("/error")]
        public IActionResult Error()
        {
            IExceptionHandlerFeature exceptionHandlerFeature = HttpContext.Features.Get<IExceptionHandlerFeature>()!;

            Exception exception = exceptionHandlerFeature.Error;
            bool isUnAuthorize = exception is Minio.Exceptions.AuthorizationException;
            if (isUnAuthorize)
            {
                return Unauthorized(exception.Message);
            }
            string message = exception.Message;
            if (exception is Minio.Exceptions.MinioException minioException)
            {
                message = minioException.ServerMessage;
            }
            return BadRequest(message);
        }

    }
}
