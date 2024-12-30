using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.DeleteSession;

public class DeleteSessionResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}