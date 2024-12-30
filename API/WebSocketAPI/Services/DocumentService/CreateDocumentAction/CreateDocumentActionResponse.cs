using System.Text.Json.Serialization;

namespace WebSocketAPI.Services.DocumentService.CreateDocumentAction;

public class CreateDocumentActionResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}