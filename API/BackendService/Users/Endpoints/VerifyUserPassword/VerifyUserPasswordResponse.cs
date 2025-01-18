using System.Text.Json.Serialization;

namespace DocumentService.Users.Endpoints.VerifyUserPassword;

public class VerifyUserPasswordResponse
{
    [JsonPropertyName("message")]
    public required string Message { get; set; }
}