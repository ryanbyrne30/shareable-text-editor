using System.Text.Json.Serialization;

namespace WebSocketAPI.Services.DocumentService.CreateSession;

public class CreateSessionRequest
{
    [JsonPropertyName("document_id")]
    public required string DocumentId { get; set; }
    
    [JsonPropertyName("socket_id")]
    public required string SocketId { get; set; }
}