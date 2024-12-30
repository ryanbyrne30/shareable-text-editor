using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WebSocketAPI.Processes.SendMessage;

public class SendMessageRequest
{
    [Required]
    [StringLength(10000, ErrorMessage = "Message is too long")]
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}