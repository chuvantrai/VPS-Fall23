using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap.AutoComplete.Detail
{
    public class Response
    {
        [JsonPropertyName("status")]
        public string? Status { get; set; }
        [JsonPropertyName("result")]
        public Result? Result { get; set; }
    }
    public class Result
    {
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = null!;
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; } = null!;
        [JsonPropertyName("geometry")]
        public GeometryDTO Geometry { get; set; } = null!;
        [JsonPropertyName("compound")]
        public Compound Compound { get; set; } = null!;
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("url")]
        public string Url { get; set; } = null!;
        [JsonPropertyName("types")]
        public string[]? Types { get; set; }
    }
}
