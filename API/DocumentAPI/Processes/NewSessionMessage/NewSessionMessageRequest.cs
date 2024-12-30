using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.NewSessionMessage;

public class NewSessionMessageRequest
{
    [Required]
    [JsonPropertyName("session_id")]
    [StringLength(36, MinimumLength = 36, ErrorMessage = "session_id must be 36 characters long.")] 
    public required string SessionId { get; set; }
    
    [Required]
    [JsonPropertyName("message")]
    [StringLength(10000, ErrorMessage = "Message is too long.")]
    public required string Message { get; set; }  
}