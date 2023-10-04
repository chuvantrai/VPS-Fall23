using VPS.MinIO.BusinessObjects.MinIO;

namespace VPS.MinIO.Repository.MinIO
{
    public interface IMinIOClient
    {
        void BuildCredential(Credential credential);
    }
}
