using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.CreateDocumentAction;

public class CreateDocumentActionRequest
{
    [JsonPropertyName("socket_id")]
    [Required(ErrorMessage = "socket_id is required")]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "socket_id must be 36 characters long")]
    public required string SocketId { get; set; }
    
    [JsonPropertyName("revision")]
    [Required(ErrorMessage = "revision is required")]
    [Range(1, long.MaxValue, ErrorMessage = "revision must be greater than 0")]
    public long Revision { get; set; }
    
    [JsonPropertyName("position")]
    [Required(ErrorMessage = "position is required")]
    [Range(0, long.MaxValue, ErrorMessage = "position must be greater than or equal to 0")]
    public long Position { get; set; }
    
    [JsonPropertyName("deleted")]
    [Required(ErrorMessage = "deleted is required")]
    [Range(0, long.MaxValue, ErrorMessage = "deleted must be greater than or equal to 0")]
    public long Deleted { get; set; }
    
    [JsonPropertyName("inserted")]
    [Required(ErrorMessage = "inserted is required")]
    [StringLength(10000, ErrorMessage = "inserted must be less than 10000 characters long")]
    public required string Inserted { get; set; }
}