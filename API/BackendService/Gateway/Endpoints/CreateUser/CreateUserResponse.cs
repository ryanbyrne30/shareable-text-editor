using System.Text.Json.Serialization;

namespace BackendService.Gateway.Endpoints.CreateUser;

public class CreateUserResponse(string id)
{
    [JsonPropertyName("id")] 
    public string Id { get; set; } = id;
}