using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.GetDocumentById;

public class GetDocumentByIdResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
    
    [JsonPropertyName("name")]
    public required string Name { get; set; }
    
    [JsonPropertyName("content")]
    public required string Content { get; set; }
}