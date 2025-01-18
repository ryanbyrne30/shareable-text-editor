using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.SignInUser;

public class SignInUserResponse
{
    [JsonPropertyName("auth_token")]
    public required string AuthToken { get; set; } 
    
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; } 
}