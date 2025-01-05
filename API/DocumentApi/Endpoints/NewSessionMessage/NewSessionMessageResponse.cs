using System.Text.Json.Serialization;

namespace DocumentAPI.Endpoints.NewSessionMessage;

public class NewSessionMessageResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}