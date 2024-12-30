using System.Text.Json.Serialization;

namespace WebSocketAPI.Processes.SendMessage;

public class SendMessageResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}