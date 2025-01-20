using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.RegisterUser;

public class RegisterUserResponse(string id)
{
    [JsonPropertyName("id")] 
    public string Id { get; set; } = id;
}