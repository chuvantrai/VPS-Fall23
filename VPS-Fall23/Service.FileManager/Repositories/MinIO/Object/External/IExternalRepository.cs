using Minio;

namespace VPS.MinIO.Repository.MinIO.Object.External
{
    public interface IExternalRepository : IMinIOClient
    {
        Task<string> GetDownloadObjectUrl(string bucket, string objectName, int expireInSecond, CancellationToken cancellationToken = default);
        Task<string> GetDownloadObjectUrl(PresignedGetObjectArgs getObjectArgs, CancellationToken cancellation = default);
        Task<string> GetPutObjectUrl(string bucket, string objectName, int expireInSecond, CancellationToken cancellationToken = default);
        Task<string> GetPutObjectUrlAsync(PresignedPutObjectArgs putObjectArgs, CancellationToken cancellation = default);
    }
}
