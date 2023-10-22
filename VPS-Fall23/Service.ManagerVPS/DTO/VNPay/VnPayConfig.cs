using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.VNPay
{
    public class VnPayConfig
    {
        [JsonPropertyName("vnp_Version")]
        [JsonProperty(propertyName: "vnp_Version")]
        public string Vnp_Version { get; set; } = null!;
        [JsonPropertyName("vnp_TmnCode")]
        [JsonProperty(propertyName: "vnp_TmnCode")]
        public string Vnp_TmnCode { get; set; } = null!;
        [JsonPropertyName("url")]
        [JsonProperty(propertyName: "url")]
        public string Url { set; get; } = null!;
        [JsonPropertyName("hashSecret")]
        [JsonProperty(propertyName: "hashSecret")]
        public string HashSecret { get; set; } = null!;
        [JsonPropertyName("vnp_ReturnUrl")]
        [JsonProperty(propertyName: "vnp_ReturnUrl")]
        public string Vnp_ReturnUrl { get; set; } = null!;
        [JsonPropertyName("vnp_CurrCode")]
        [JsonProperty(propertyName: "vnp_CurrCode")]
        public string Vnp_CurrCode { get; set; } = null!;
        [JsonPropertyName("vnp_Locale")]
        [JsonProperty(propertyName: "vnp_Locale")]
        public string Vnp_Locale { get; set; } = null!;
        [JsonPropertyName("vnp_OrderType")]
        [JsonProperty(propertyName: "vnp_OrderType")]
        public string Vnp_OrderType { get; set; } = null!;
        [JsonProperty(propertyName: "expireMinutes")]
        [JsonPropertyName("expireMinutes")]
        public int ExpireMinutes { get; set; }
    }
}
