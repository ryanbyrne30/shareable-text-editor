using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.SignInUser;

public class SignInUserResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; } 
    
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; } 
}