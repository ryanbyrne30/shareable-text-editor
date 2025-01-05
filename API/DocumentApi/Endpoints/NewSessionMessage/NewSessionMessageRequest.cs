using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Endpoints.NewSessionMessage;

public class NewSessionMessageRequest
{
    [Required]
    [JsonPropertyName("message")]
    [StringLength(10000, ErrorMessage = "Message is too long.")]
    public required string Message { get; set; }
}