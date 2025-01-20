using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.GetUserById;

public class GetUserByIdResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; } 
    
    [JsonPropertyName("username")]
    public required string Username { get; set; }
}