using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap.AutoComplete
{
    public class Response
    {
        [JsonPropertyName("predictions")]

        public List<Prediction> Predictions { get; set; }
        [JsonPropertyName("executed_time")]
        public int ExecutedTime { get; set; }
        [JsonPropertyName("executed_time_all")]

        public int ExecutedTimeAll { get; set; }
        [JsonPropertyName("status")]

        public string? Status { get; set; }

    }
    public class Prediction
    {
        [JsonPropertyName("description")]
        public string Description { get; set; } = null!;
        [JsonPropertyName("matched_substrings")]

        public List<MatchedSubString>? MatchedSubstrings { get; set; }
        [JsonPropertyName("place_id")]

        public string PlaceId { get; set; } = null!;
        [JsonPropertyName("reference")]

        public string References { get; set; } = null!;
        [JsonPropertyName("structured_formatting")]

        public StructuredFormatting StructuredFormatting { get; set; } = null!;
        [JsonPropertyName("terms")]

        public List<Term>? Terms { get; set; }
        [JsonPropertyName("has_children")]

        public bool HasChildren { get; set; }
        [JsonPropertyName("display_type")]

        public string? DisplayType { get; set; }
        [JsonPropertyName("types")]
        public string[]? Types { get; set; }

        [JsonPropertyName("score")]

        public double? Score { get; set; }
        [JsonPropertyName("distance_meters")]
        public double? DistanceMeters { get; set; }

        [JsonPropertyName("plus_code")]
        public PlusCode PlusCode { get; set; } = null!;

        [JsonPropertyName("compound")]
        public Compound? Compound { get; set; }

    }
}
