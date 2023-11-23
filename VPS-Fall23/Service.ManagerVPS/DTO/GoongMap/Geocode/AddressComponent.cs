using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.GoongMap.Geocode
{
    public class AddressComponent
    {
        [JsonPropertyName("long_name")]
        public string LongName { get; set; } = null!;
        [JsonPropertyName("short_name")]
        public string ShortName { get; set; } = null!;
    }
}
