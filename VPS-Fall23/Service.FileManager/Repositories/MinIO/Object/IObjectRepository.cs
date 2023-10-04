using Minio;
using Minio.DataModel;

namespace VPS.MinIO.Repository.MinIO.Object
{
    public interface IObjectRepository : IMinIOClient
    {
        Task<ObjectStat> GetObject(GetObjectArgs getObjectArgs, CancellationToken cancellation = default);

        Task<PutObjectResponse> PutObjectAsync(PutObjectArgs putObjectArgs, CancellationToken cancellationToken = default);

        Task<PutObjectResponse> PutObject(string bucket, string objectName, MemoryStream memoryStream, CancellationToken cancellationToken = default);

        Task<IList<Item>> ListObjects(string bucketName, string? prefix = null, bool recusive = false, CancellationToken cancellationToken = default);

        Task<IList<Item>> ListObjects(ListObjectsArgs listObjectsArgs, CancellationToken cancellationToken = default);

        Task RemoveObject(RemoveObjectArgs removeObjectArgs, CancellationToken cancellationToken = default);
        Task RemoveObjects(RemoveObjectsArgs removeObjectsArgs, CancellationToken cancellationToken = default);
        Task<bool> IsObjectExist(StatObjectArgs statObjectArgs, CancellationToken cancellationToken = default);
    }
}
