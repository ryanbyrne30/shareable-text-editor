using System.Text.Json.Serialization;

namespace DocumentAPI.Services.WebSocketApiService.SendMessageToSocket;

public class SendMessageToSocketResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}