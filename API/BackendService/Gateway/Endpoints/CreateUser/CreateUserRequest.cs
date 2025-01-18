using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BackendService.Gateway.Attributes;

namespace BackendService.Gateway.Endpoints.CreateUser;

public sealed class CreateUserRequest(string username, string password)
{
    [Required]
    [UsernameValidation]
    [JsonPropertyName("username")]
    public string Username { get; set; } = username;

    [Required]
    [PasswordValidation]
    [JsonPropertyName("password")]
    public string Password { get; set; } = password;
};