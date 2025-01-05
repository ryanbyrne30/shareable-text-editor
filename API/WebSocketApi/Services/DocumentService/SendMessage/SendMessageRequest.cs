using System.Text.Json.Serialization;

namespace WebSocketAPI.Services.DocumentService.SendMessage;

public class SendMessageRequest
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}