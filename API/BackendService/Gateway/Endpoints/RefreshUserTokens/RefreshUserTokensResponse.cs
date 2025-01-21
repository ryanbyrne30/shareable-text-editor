using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.RefreshUserTokens;

public class RefreshUserTokensResponse
{
    [JsonPropertyName("access_token")]
    public required string AccessToken { get; set; } 
    
    [JsonPropertyName("refresh_token")]
    public required string RefreshToken { get; set; } 
}