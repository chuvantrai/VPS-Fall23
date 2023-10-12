using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Minio;
using Minio.DataModel;
using VPS.MinIO.API.Controllers.Base;
using VPS.MinIO.BusinessObjects;
using VPS.MinIO.Repository.MinIO.Object;

namespace VPS.MinIO.API.Controllers
{

    [Route("api/{bucketName}/object")]
    public class ObjectController : BaseApiController
    {
        private readonly IObjectRepository objectRepository;
        private readonly IMapper mapper;
        public ObjectController
            (
            IObjectRepository objectRepository,
            IMapper mapper
            )
        {
            this.objectRepository = objectRepository;
            this.mapper = mapper;
        }

        [HttpPost]
        public void PutObject(
           IFormFileCollection files,
            string bucketName,
            string? folderPath = null,
            CancellationToken cancellationToken = default)
        {
            if (files.Count == 0)
            {
                throw new ArgumentNullException(nameof(files), "Please send at least one file");
            }
            files.AsParallel().ForAll(async file =>
               {
                   using Stream stream = file.OpenReadStream();
                   PutObjectArgs putObjectArgs = new PutObjectArgs()
                       .WithBucket(bucketName)
                       .WithObject($"{folderPath}/{file.FileName}")
                       .WithObjectSize(file.Length)
                       .WithStreamData(stream);
                   PutObjectResponse putObjectResponse = await objectRepository.PutObjectAsync(putObjectArgs, cancellationToken);
               });
        }

        [HttpGet]
        public async Task<IList<Item>> GetAllObject(
            string bucketName,
            string? prefix = null,
            bool recursive = false, CancellationToken cancellationToken = default)
        {

            IList<Item> result = await objectRepository.ListObjects(bucketName, prefix, recursive, cancellationToken);
            return result;
        }

        [HttpGet("get-one")]
        public async Task<IActionResult> GetBytes(
            string bucketName,
            string objectName,
            string? eTag = null,
            string? versionId = null,
            bool download = false,
            CancellationToken cancellationToken = default)
        {
            using MemoryStream memoryStream = new();
            GetObjectArgs getObjectArgs = new GetObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithMatchETag(eTag)
                .WithVersionId(versionId)
                .WithCallbackStream(stream => stream.CopyTo(memoryStream));

            ObjectStat objectStat = await objectRepository.GetObject(getObjectArgs, cancellationToken);
            if (download)
            {
                byte[] bytes = memoryStream.ToArray();
                return File(bytes, objectStat.ContentType);
            }
            return Ok(objectStat);

        }

        [HttpDelete]
        public async Task<IActionResult> Delete(
            string bucketName,
            string objectName,
            string? versionId = null,
            CancellationToken cancellationToken = default
            )
        {
            RemoveObjectArgs removeObjectArgs = new RemoveObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithVersionId(versionId);
            await objectRepository.RemoveObject(removeObjectArgs, cancellationToken);
            return NoContent();
        }
        [HttpDelete("multiple")]
        public async Task<IActionResult> MultipleDelete
            (
            string bucketName,
            IList<RemoveObjectsDto> removeObjectsDtos,
            CancellationToken cancellation = default
            )
        {
            RemoveObjectsArgs removeObjectsArgs = new RemoveObjectsArgs()
                .WithBucket(bucketName)
                .WithObjects(removeObjectsDtos.Where(r => r.VersionsId.Length == 0).Select(r => r.ObjectName).ToArray())
                .WithObjectsVersions(removeObjectsDtos.Where(r => r.VersionsId.Length > 0).Select(r => r.ToTuple()).ToList());
            await objectRepository.RemoveObjects(removeObjectsArgs, cancellation);
            return NoContent();
        }
        [HttpGet("exist")]
        public async Task<bool> Exist(
            string bucketName,
            string objectName,
            string? eTag = null,
            string? versionId = null,
            CancellationToken cancellation = default
            )
        {
            StatObjectArgs statObjectArgs = new StatObjectArgs()
                .WithBucket(bucketName)
                .WithObject(objectName)
                .WithMatchETag(eTag)
                .WithVersionId(versionId);
            bool result = await objectRepository.IsObjectExist(statObjectArgs, cancellation);
            return result;
        }
    }
}
