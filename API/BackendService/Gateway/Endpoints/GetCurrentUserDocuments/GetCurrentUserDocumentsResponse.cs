using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.GetCurrentUserDocuments;

public class GetCurrentUserDocumentsResponse
{
    [JsonPropertyName("documents")]
    public required List<DocumentType> Documents { get; set; } 
    
    [JsonPropertyName("total")]
    public int Total { get; set; }

    public class DocumentType
    {
        [JsonPropertyName("id")]
        public required string Id { get; set; }
        
        [JsonPropertyName("name")]
        public required string Name { get; set; }
        
        [JsonPropertyName("created_at")]
        public DateTime CreatedAt { get; set; }
        
        [JsonPropertyName("updated_at")]
        public DateTime? UpdatedAt { get; set; }
    }
}