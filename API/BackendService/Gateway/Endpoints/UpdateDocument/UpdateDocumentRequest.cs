using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.UpdateDocument;

public class UpdateDocumentRequest
{
    [JsonPropertyName("name")]
    [MaxLength(100)]
    public string? Name { get; set; }
    
    [JsonPropertyName("content")]
    [MaxLength(100000)]
    public string? Content { get; set; }
}