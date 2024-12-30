using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.CreateSession;

public class CreateSessionRequest
{
    [Required]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "DocumentId must be 36 characters long")]
    [JsonPropertyName("document_id")]
    public required string DocumentId { get; set; }
    
    [Required]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "SocketId must be 36 characters long")]
    [JsonPropertyName("socket_id")]
    public required string SocketId { get; set; }
}