using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap.Geocode
{
    public class Response
    {
        [JsonPropertyName("results")]
        public List<ResultDTO> Results { get; set; } = null!;
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
    }
    public class ResultDTO
    {
        [JsonPropertyName("address_components")]
        public List<AddressComponent> AddressComponents { get; set; } = null!;
        [JsonPropertyName("formatted_address")]
        public string FormattedAddress { get; set; } = null!;
        [JsonPropertyName("geometry")]
        public GeometryDTO Geometry { get; set; } = null!;
        [JsonPropertyName("place_id")]
        public string PlaceId { get; set; } = null!;
        [JsonPropertyName("reference")]
        public string Reference { get; set; } = null!;
        [JsonPropertyName("plus_code")]
        public PlusCode PlusCode { get; set; } = null!;
        [JsonPropertyName("compound")]
        public Compound Compound { get; set; } = null!;
        [JsonPropertyName("types")]
        public string[] Types { get; set; } = null!;
        [JsonPropertyName("name")]
        public string Name { get; set; } = null!;
        [JsonPropertyName("address")]
        public string Address { get; set; } = null!;

    }
}
