using Minio;

namespace VPS.MinIO.Repository.MinIO.Object.External
{
    public class ExternalRepository : MinIOClient, IExternalRepository
    {
        public ExternalRepository(string endPoint, int port, bool useSSL = false)
            : base(endPoint, port, useSSL)
        {

        }
        public async Task<string> GetDownloadObjectUrl
            (
            string bucket,
            string objectName,
            int expireInSecond,
            CancellationToken cancellationToken = default
            )
        {
            PresignedGetObjectArgs getObjectArgs = new PresignedGetObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithExpiry(expireInSecond);
            string url = await GetDownloadObjectUrl(getObjectArgs, cancellationToken);
            return url;
        }

        public async Task<string> GetDownloadObjectUrl
            (
            PresignedGetObjectArgs getObjectArgs,
            CancellationToken cancellation = default
            )
        {
            string url = await client.PresignedGetObjectAsync(getObjectArgs);
            return url;
        }

        public async Task<string> GetPutObjectUrl
            (
            string bucket,
            string objectName,
            int expireInSecond,
            CancellationToken cancellationToken = default
            )
        {
            PresignedPutObjectArgs putObjectArgs = new PresignedPutObjectArgs()
                .WithBucket(bucket)
                .WithObject(objectName)
                .WithExpiry(expireInSecond);
            string url = await GetPutObjectUrlAsync(putObjectArgs, cancellationToken);
            return url;
        }
        public async Task<string> GetPutObjectUrlAsync
            (
            PresignedPutObjectArgs putObjectArgs,
            CancellationToken cancellation = default
            )
        {
            string url = await client.PresignedPutObjectAsync(putObjectArgs);
            return url;
        }
    }
}
