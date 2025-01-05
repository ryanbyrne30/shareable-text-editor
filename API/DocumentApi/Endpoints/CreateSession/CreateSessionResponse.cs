using System.Text.Json.Serialization;

namespace DocumentAPI.Endpoints.CreateSession;

public class CreateSessionResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; }
}