using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap
{
    public class Compound
    {
        [JsonPropertyName("district")]
        public string District { get; set; } = null!;
        [JsonPropertyName("commune")]
        public string Commune { get; set; } = null!;
        [JsonPropertyName("province")]
        public string Provine { get; set; } = null!;
    }
}
