
using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap
{
    public class GeometryDTO
    {
        [JsonPropertyName("location")]
        public Position Position { get; set; } = null!;
        [JsonPropertyName("boundary")]
        public string? Boundary { get; set;}
    }
}
