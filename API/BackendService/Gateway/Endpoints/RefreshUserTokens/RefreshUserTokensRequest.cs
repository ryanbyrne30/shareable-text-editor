using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.RefreshUserTokens;

public class RefreshUserTokensRequest
{
    
    [JsonPropertyName("refresh_token")]
    [MaxLength(2048)]
    public required string RefreshToken { get; set; }
}