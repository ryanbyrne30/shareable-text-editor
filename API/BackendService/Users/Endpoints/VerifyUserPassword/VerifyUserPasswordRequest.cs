using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentService.Users.Endpoints.VerifyUserPassword;

public class VerifyUserPasswordRequest
{
    [JsonPropertyName("username")]
    [MaxLength(100)]
    public required string Username { get; set; }
    
    [JsonPropertyName("password")]
    [MaxLength(100)]
    public required string Password { get; set; }
}