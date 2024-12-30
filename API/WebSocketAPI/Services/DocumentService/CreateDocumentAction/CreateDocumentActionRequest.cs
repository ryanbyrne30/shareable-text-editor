using System.Text.Json.Serialization;

namespace WebSocketAPI.Services.DocumentService.CreateDocumentAction;

public class CreateDocumentActionRequest
{
    [JsonPropertyName("socket_id")]
    public required string SocketId { get; set; }
    
    [JsonPropertyName("revision")]
    public long Revision { get; set; }
    
    [JsonPropertyName("position")]
    public long Position { get; set; }
    
    [JsonPropertyName("deleted")]
    public long Deleted { get; set; }
    
    [JsonPropertyName("inserted")]
    public required string Inserted { get; set; }
}