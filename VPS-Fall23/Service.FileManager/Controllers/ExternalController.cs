using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using VPS.MinIO.API.Controllers.Base;
using VPS.MinIO.BusinessObjects.AppSetting;
using VPS.MinIO.Repository.MinIO.Object.External;

namespace VPS.MinIO.API.Controllers
{
    [Route("api/external")]
    public class ExternalController : BaseApiController
    {
        private readonly IExternalRepository externalRepository;
        private readonly AppSetting appSetting;
        public ExternalController
            (
            IExternalRepository externalRepository,
            IOptions<AppSetting> appSetting
            )
        {
            this.externalRepository = externalRepository;
            this.appSetting = appSetting.Value;
        }
        [HttpGet("get-download-url")]
        public async Task<string> GetDownloadUrl(
        string bucketName,
        string objectName,
           CancellationToken cancellationToken = default)
        {
            string url = await externalRepository.GetDownloadObjectUrl(bucketName, objectName, appSetting.MinIO.Presign.ExpireInSecond, cancellationToken);
            return url;
        }
        [HttpGet("get-put-url")]
        public async Task<string> GetPutUrl(
        string bucketName,
        string objectName,
            CancellationToken cancellationToken = default)
        {
            string url = await externalRepository.GetPutObjectUrl(bucketName, objectName, appSetting.MinIO.Presign.ExpireInSecond, cancellationToken);
            return url;
        }
    }
}
