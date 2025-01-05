using System.Text.Json.Serialization;

namespace WebSocketAPI.Services.DocumentService.DeleteSession;

public class DeleteSessionResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}