using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.AppSetting
{
    public class FileManagementConfig
    {
        [JsonPropertyName("baseUrl")]
        public string BaseUrl { get; set; }
        [JsonPropertyName("endPointServer")]
        public string EndPointServer { get; set; }
        [JsonPropertyName("accessKey")]
        public string AccessKey { get; set; }
        [JsonPropertyName("secretKey")]
        public string SecretKey { get; set; }

        [JsonPropertyName("endPointPort")]
        public FileManagementPort EndPointPort { get; set; }

        [JsonPropertyName("publicBucket")]
        public string PublicBucket { get; set; }
        [JsonPropertyName("privateBucket")]
        public string PrivateBucket { get; set; }

    }
    public class FileManagementPort
    {
        [JsonPropertyName("api")]
        public int Api { get; set; }
        [JsonPropertyName("access")]
        public int Access { get; set; }
    }
}
