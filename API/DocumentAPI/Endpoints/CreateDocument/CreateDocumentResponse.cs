using System.Text.Json.Serialization;

namespace DocumentAPI.Endpoints.CreateDocument;

public class CreateDocumentResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}