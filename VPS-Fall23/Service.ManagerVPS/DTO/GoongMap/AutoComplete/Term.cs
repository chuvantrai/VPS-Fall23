using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap.AutoComplete
{
    public class Term
    {
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
