using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.SignInUser;

public class SignInUserRequest
{
    [JsonPropertyName("username")]
    [MaxLength(100)]
    public required string Username { get; set; }
    
    [JsonPropertyName("password")]
    [MaxLength(100)]
    public required string Password { get; set; }
}