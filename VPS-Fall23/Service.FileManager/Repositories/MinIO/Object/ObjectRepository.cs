using Minio;
using Minio.DataModel;
using System.Reactive.Linq;

namespace VPS.MinIO.Repository.MinIO.Object
{
    internal class ObjectRepository : MinIOClient, IObjectRepository
    {
        public ObjectRepository(string endPoint, int port, bool useSSL)
            : base(endPoint, port, useSSL)
        {

        }
        public async Task<ObjectStat> GetObject
            (
            GetObjectArgs getObjectArgs,
            CancellationToken cancellation = default
            )
        {
            ObjectStat objectStat = await client.GetObjectAsync(getObjectArgs, cancellation);
            return objectStat;
        }
        public async Task<PutObjectResponse> PutObjectAsync
            (
            PutObjectArgs putObjectArgs,
            CancellationToken cancellationToken = default
            )
        {
            PutObjectResponse putObjectResponse = await client.PutObjectAsync(putObjectArgs, cancellationToken);
            return putObjectResponse;
        }
        public async Task<PutObjectResponse> PutObject
            (
            string bucket,
            string objectName,
            MemoryStream memoryStream,
            CancellationToken cancellationToken = default
            )
        {
            PutObjectArgs putObjectArgs = new PutObjectArgs()
                .WithBucket(bucket)
                .WithFileName(objectName)
                .WithStreamData(memoryStream);
            PutObjectResponse putObjectResponse = await PutObjectAsync(putObjectArgs, cancellationToken);
            return putObjectResponse;
        }


        public Task<IList<Item>> ListObjects
            (
            string bucketName,
            string? prefix = null,
            bool recusive = false,
            CancellationToken cancellationToken = default
            )
        {
            ListObjectsArgs listObjectsArgs = new ListObjectsArgs()
                .WithBucket(bucketName)
                .WithPrefix(prefix)
                .WithRecursive(recusive);

            return ListObjects(listObjectsArgs, cancellationToken);
        }
        public Task<IList<Item>> ListObjects
            (
            ListObjectsArgs listObjectsArgs,
            CancellationToken cancellationToken = default
            )
        {
            IObservable<Item> observable = client.ListObjectsAsync(listObjectsArgs, cancellationToken);
            IList<Item> items = observable.ToList().Wait();
            return Task.FromResult(items);
        }

        public async Task RemoveObject
            (
            RemoveObjectArgs removeObjectArgs,
            CancellationToken cancellationToken = default
            )
        {
            await client.RemoveObjectAsync(removeObjectArgs, cancellationToken);
        }
        public async Task RemoveObjects
            (
            RemoveObjectsArgs removeObjectsArgs,
            CancellationToken cancellationToken = default
            )
        {
            await client.RemoveObjectsAsync(removeObjectsArgs, cancellationToken);
        }

        public async Task<bool> IsObjectExist(StatObjectArgs statObjectArgs, CancellationToken cancellationToken = default)
        {
            try
            {
                await client.StatObjectAsync(statObjectArgs, cancellationToken);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
