using Microsoft.AspNetCore.Mvc;
using Minio.DataModel;
using VPS.MinIO.API.Controllers.Base;
using VPS.MinIO.BusinessObjects;
using VPS.MinIO.Repository.MinIO.Bucket;

namespace VPS.MinIO.API.Controllers
{
    [Route("api/bucket")]
    public class BucketController : BaseApiController
    {
        private readonly IBucketRepository bucketRepository;
        public BucketController(IBucketRepository bucketRepository)
        {
            this.bucketRepository = bucketRepository;

        }
        [HttpGet()]
        public async Task<ListAllMyBucketsResult> GetAllBuckets(CancellationToken cancellationToken = default)
        {
            ListAllMyBucketsResult listAllMyBucketsResult = await bucketRepository.ListBuckets(cancellationToken);
            return listAllMyBucketsResult;
        }
        [HttpPost]
        public async Task CreateBucket(BucketDto bucketDto, CancellationToken cancellationToken = default)
        {
            await bucketRepository.MakeBucket(bucketDto.BucketName, cancellationToken);
        }

        [HttpGet("{bucketName}/exist")]
        public async Task<bool> IsExist(string bucketName, CancellationToken cancellationToken = default)
        {
            bool isExist = await bucketRepository.IsBucketExist(bucketName, cancellationToken);
            return isExist;
        }
        [HttpDelete("{bucketName}")]
        public async Task RemoveBucket(string bucketName, CancellationToken cancellationToken = default)
        {
            await bucketRepository.RemoveBucket(bucketName, cancellationToken);
        }
    }
}
