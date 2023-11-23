using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap
{
    public class PlusCode
    {
        [JsonPropertyName("compound_code")]
        public string? CompoundCode { get; set; }
        [JsonPropertyName("global_code")]
        public string? GlobalCode { get; set; }
    }
}
