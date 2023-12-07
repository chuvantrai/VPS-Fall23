using System.Text.Json.Serialization;

namespace Service.ManagerVPS.DTO.OtherModels;

public class TwilioSettings
{
    [JsonPropertyName("accountSid")]
    public string AccountSid { get; set; }
    
    [JsonPropertyName("authToken")]
    public string AuthToken { get; set; }
    
    [JsonPropertyName("twilioPhoneNumber")]
    public string TwilioPhoneNumber { get; set; }
}