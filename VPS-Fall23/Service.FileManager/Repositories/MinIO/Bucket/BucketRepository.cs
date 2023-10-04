using Minio.DataModel;
using Minio;

namespace VPS.MinIO.Repository.MinIO.Bucket
{
    public class BucketRepository : MinIOClient, IBucketRepository
    {
        public BucketRepository(string endPoint, int port, bool useSSL = false)
        : base(endPoint, port, useSSL)
        {
        }

        public async Task MakeBucket(string bucketName, CancellationToken cancellationToken = default)
        {
            MakeBucketArgs args = new MakeBucketArgs().WithBucket(bucketName);
            await MakeBucket(args, cancellationToken);
        }
        public async Task MakeBucket(MakeBucketArgs makeBucketArgs, CancellationToken cancellationToken = default)
        {
            await client.MakeBucketAsync(makeBucketArgs, cancellationToken);
        }

        public async Task<ListAllMyBucketsResult> ListBuckets(CancellationToken cancellationToken = default)
        {
            ListAllMyBucketsResult listAllMyBucketsResult = await client.ListBucketsAsync(cancellationToken);
            return listAllMyBucketsResult;
        }

        public async Task<bool> IsBucketExist(BucketExistsArgs bucketExistsArgs, CancellationToken cancellationToken = default)
        {
            bool isExist = await client.BucketExistsAsync(bucketExistsArgs, cancellationToken);
            return isExist;
        }
        public async Task<bool> IsBucketExist(string bucketName, CancellationToken cancellationToken = default)
        {
            BucketExistsArgs makeBucketArgs = new BucketExistsArgs().WithBucket(bucketName);
            return await IsBucketExist(makeBucketArgs, cancellationToken);
        }

        public async Task RemoveBucket(string bucketName, CancellationToken cancellationToken = default)
        {
            RemoveBucketArgs removeBucketArgs = new RemoveBucketArgs()
                .WithBucket(bucketName);
            await RemoveBucket(removeBucketArgs, cancellationToken);

        }
        public async Task RemoveBucket(RemoveBucketArgs removeBucketArgs, CancellationToken cancellation = default)
        {
            await client.RemoveBucketAsync(removeBucketArgs, cancellation);
        }
    }
}
