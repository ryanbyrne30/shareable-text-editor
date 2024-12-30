using System.Text.Json.Serialization;

namespace WebSocketAPI.Services.DocumentService.CreateSession;

public class CreateSessionResponse
{
    [JsonPropertyName("id")]
    public required string Id{ get; set; }
}