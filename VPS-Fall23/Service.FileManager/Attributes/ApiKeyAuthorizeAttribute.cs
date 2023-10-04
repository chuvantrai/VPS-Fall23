using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Primitives;
using VPS.Helper;
using VPS.MinIO.BusinessObjects.MinIO;
using VPS.MinIO.Repository.MinIO.Bucket;
using VPS.MinIO.Repository.MinIO.Object;

namespace VPS.MinIO.API.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    internal class ApiKeyAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        internal static bool IsHaveMinIOAccessKey(HttpRequest httpRequest, out Credential credential)
        {
            bool isHaveAccessKey = httpRequest.Headers.TryGetValue(Constant.MINIO_ACCESS_KEY_HEADER_NAME, out StringValues accessKey);
            bool isHaveSecretKey = httpRequest.Headers.TryGetValue(Constant.MINIO_SECRET_KEY_HEADER_NAME, out StringValues secretKey);
            credential = new Credential()
            {
                AccessKey = accessKey.ToString(),
                SecretKey = secretKey.ToString()
            };
            return isHaveAccessKey && isHaveSecretKey;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                bool isHaveMinIOAccessKey = IsHaveMinIOAccessKey(context.HttpContext.Request, out Credential credential);
                if (!isHaveMinIOAccessKey)
                {
                    context.Result = new UnauthorizedResult();
                    return;
                }
                context.HttpContext.RequestServices.GetRequiredService<IBucketRepository>().BuildCredential(credential);
                context.HttpContext.RequestServices.GetRequiredService<IObjectRepository>().BuildCredential(credential);
            }
            catch (Exception)
            {
                context.Result = new UnauthorizedResult();
                return;
            }


        }
    }
}
