using Microsoft.AspNetCore.Mvc;
using VPS.Helper;
using VPS.MinIO.API.Attributes;
using VPS.MinIO.BusinessObjects.MinIO;

namespace VPS.MinIO.API.Controllers.Base
{
    [ApiKeyAuthorize()]
    [ApiController]
    public class BaseApiController : ControllerBase
    {
        protected Credential Credential
        {
            get
            {
                bool isHaveMinIOAccessKey = ApiKeyAuthorizeAttribute.IsHaveMinIOAccessKey(this.Request, out Credential credential);
                if (!isHaveMinIOAccessKey)
                {
                    throw new Minio.Exceptions.AuthorizationException(Constant.MINIO_ACCESS_KEY_NULL_MSG);
                }
                return credential;
            }
        }
    }
}
