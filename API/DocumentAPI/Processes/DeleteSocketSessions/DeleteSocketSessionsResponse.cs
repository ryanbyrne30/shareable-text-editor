using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.DeleteSocketSessions;

public class DeleteSocketSessionsResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}