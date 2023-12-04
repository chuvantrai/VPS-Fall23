using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap.AutoComplete
{

    public class MatchedSubString
    {
        [JsonPropertyName("length")]
        public int Length { get; set; }
        [JsonPropertyName("offset")]
        public int Offset { get; set; }
    }
}
