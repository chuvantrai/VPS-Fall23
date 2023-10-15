using System.Net.Http.Headers;
using InvoiceApi.ExternalData;
using Service.ManagerVPS.DTO.FileManagement;

namespace Service.ManagerVPS.ExternalClients
{
    public class FileManagementClient
    {
        public const string MULTIPART_FORM_PARAM_NAME = "files";
        readonly RestClient restClient;
        const string BUCKET_URI = "api/bucket";
        const string CHECK_EXIST_BUCKET_URI = "api/bucket/{0}/exist";
        const string OBJECT_URI = "api/{0}/object";

        const string OBJECT_GET_ONE_URI =
            "api/{0}/object/get-one?objectName={1}&eTag={2}&versionId={3}&download={4}";

        const string OBJECT_DELETE_MULTIPLE = "api/{0}/object/multiple";

        const string OBJECT_EXIST_URI =
            "api/{0}/object/exist?objectName={1}&eTag={2}&versionId={3}";
        public FileManagementClient(string baseUrl)
        {
            restClient = new RestClient(baseUrl);
        }
        
        public FileManagementClient(string baseUrl, string accessKey, string secretKey)
            : this(baseUrl)
        {
            restClient.DefaultRequestHeaders.TryAddWithoutValidation("minio-access-key", accessKey);
            restClient.DefaultRequestHeaders.TryAddWithoutValidation("minio-secret-key", secretKey);
        }

        public async Task<List<ListObjectsDto>> GetObjects(string bucket, string prefix = null,
            bool recursive = false)
        {
            string uri = string.Format(OBJECT_URI, bucket);
            uri = $"{uri}?prefix={prefix}&recursive={recursive}";
            return await restClient.Get<List<ListObjectsDto>>(uri);
        }


        public async Task Upload(string bucket, string folderPath, byte[] fileBytes,
            string fileName)
        {
            using MemoryStream memoryStream = new MemoryStream(fileBytes);
            StreamContent streamContent = new StreamContent(memoryStream);
            MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent
            {
                { streamContent, MULTIPART_FORM_PARAM_NAME, fileName }
            };
            await Upload(bucket, folderPath, multipartFormDataContent);
            streamContent.Dispose();
            multipartFormDataContent.Dispose();
        }
        public async Task Upload(string bucket, string folderPath,
            MultipartFormDataContent multipartFormDataContent)
        {
            string uri = $"{string.Format(OBJECT_URI, bucket)}?folderPath={folderPath}";
            HttpResponseMessage httpResponseMessage =
                await restClient.PostAsync(uri, multipartFormDataContent);
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(httpResponseMessage.ReasonPhrase);
            }
        }
        public async Task<(byte[] respones, MediaTypeHeaderValue contentType)> Download(
            string bucket,
            string objectName,
            string eTag = "",
            string versionId = "")
        {
            var response = await GetOne(bucket, objectName, eTag, versionId, true);
            return (await response.Content.ReadAsByteArrayAsync(),
                response.Content.Headers.ContentType);
        }

        public async Task<GetOneDto> GetOne(string bucket,
            string objectName,
            string eTag = "",
            string versionId = "")
        {
            var response = await GetOne(bucket, objectName, eTag, versionId, false);
            return await response.Content.ReadFromJsonAsync<GetOneDto>();
        }
        
        async Task<HttpResponseMessage> GetOne(string bucket,
            string objectName,
            string eTag = "",
            string versionId = "",
            bool download = false)
        {
            string uri =
                $"{string.Format(OBJECT_GET_ONE_URI, bucket, objectName, eTag, versionId, download)}";
            HttpResponseMessage httpResponseMessage = (await restClient.GetAsync(uri));
            if (!httpResponseMessage.IsSuccessStatusCode)
            {
                throw new Exception(httpResponseMessage.ReasonPhrase);
            }

            return httpResponseMessage;
        }

        public async Task<bool> IsObjectExist(string bucketName,
            string objectName,
            string eTag = "",
            string versionId = "")
        {
            string uri = string.Format(OBJECT_EXIST_URI, bucketName, objectName, eTag, versionId);
            return await restClient.Get<bool>(uri);
        }

        public async Task RemoveObject(string bucketName,
            string objectName,
            string versionId = "")
        {
            string uri = string.Format(OBJECT_URI, bucketName);
            uri = $"{uri}?objectName={objectName}&versionId={versionId}";
            HttpResponseMessage message = await restClient.DeleteAsync(uri);
            if (!message.IsSuccessStatusCode)
            {
                throw new Exception(message.ReasonPhrase);
            }
        }
    }
}