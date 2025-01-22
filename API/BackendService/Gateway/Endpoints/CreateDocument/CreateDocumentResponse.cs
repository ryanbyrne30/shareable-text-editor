using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.CreateDocument;

public class CreateDocumentResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; init; }
}