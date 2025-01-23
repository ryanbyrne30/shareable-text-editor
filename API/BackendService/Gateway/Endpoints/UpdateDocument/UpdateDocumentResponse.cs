using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.UpdateDocument;

public class UpdateDocumentResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}