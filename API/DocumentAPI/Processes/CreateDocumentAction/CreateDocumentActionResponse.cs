using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.CreateDocumentAction;

public class CreateDocumentActionResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}