using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap.AutoComplete
{
    public class StructuredFormatting
    {
        [JsonPropertyName("main_text")]
        public string? MainText { get; set; }
        [JsonPropertyName("main_text_matched_substrings")]
        public List<MatchedSubString>? MainTextMatchedSubStrings { get; set; }

        [JsonPropertyName("secondary_text")]
        public string? SecondaryText { get; set; }
        [JsonPropertyName("secondary_text_matched_substrings")]
        public List<MatchedSubString>? SecondaryMatchedSubstrings { get; set; }
    }

}
