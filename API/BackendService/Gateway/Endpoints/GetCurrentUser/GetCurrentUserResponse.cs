using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.GetCurrentUser;

public class GetCurrentUserResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; } 
    
    [JsonPropertyName("username")]
    public required string Username { get; set; }
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; }
    
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; }
}