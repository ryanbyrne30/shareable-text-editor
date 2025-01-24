using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.DeleteDocument;

public class DeleteDocumentResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}