using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Services.WebSocketApiService.SendMessageToSocket;

public class SendMessageToSocketRequest
{
    [Required]
    [StringLength(10000, ErrorMessage = "Message is too long")]
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}