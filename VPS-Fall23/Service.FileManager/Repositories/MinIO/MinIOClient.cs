using Minio;
using VPS.MinIO.BusinessObjects.MinIO;

namespace VPS.MinIO.Repository.MinIO
{
    public abstract class MinIOClient : IMinIOClient
    {
        protected MinioClient client { get; private set; }
        public MinIOClient(string endPoint, int port, bool useSSL = false)
        {
            client = new MinioClient().WithEndpoint(endPoint, port).WithSSL(useSSL);
        }
        public void BuildCredential(Credential credential)
        {
            client = client.WithCredentials(credential.AccessKey, credential.SecretKey).Build();
        }
    }
}
