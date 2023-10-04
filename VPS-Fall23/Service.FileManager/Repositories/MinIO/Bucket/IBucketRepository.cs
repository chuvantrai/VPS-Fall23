using Minio;
using Minio.DataModel;

namespace VPS.MinIO.Repository.MinIO.Bucket
{
    public interface IBucketRepository : IMinIOClient
    {
        Task MakeBucket(string bucketName, CancellationToken cancellationToken = default);
        Task MakeBucket(MakeBucketArgs makeBucketArgs, CancellationToken cancellationToken = default);
        Task<ListAllMyBucketsResult> ListBuckets(CancellationToken cancellationToken = default);
        Task<bool> IsBucketExist(BucketExistsArgs bucketExistsArgs, CancellationToken cancellationToken = default);
        Task<bool> IsBucketExist(string bucketName, CancellationToken cancellationToken = default);
        Task RemoveBucket(string bucketName, CancellationToken cancellationToken  = default);

    }
}
