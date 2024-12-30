using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.CreateDocument;

public class CreateDocumentResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}