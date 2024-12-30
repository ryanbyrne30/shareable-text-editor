using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.CreateSession;

public class CreateSessionResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}