using System.Text.Json.Serialization;

namespace WebSocketAPI.Services.DocumentService.DeleteSocketSessions;

public class DeleteSocketSessionsResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}