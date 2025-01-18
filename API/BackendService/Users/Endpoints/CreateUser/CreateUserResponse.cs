using System.Text.Json.Serialization;

namespace DocumentService.Users.Endpoints.CreateUser;

public class CreateUserResponse(string id)
{
    [JsonPropertyName("id")] 
    public string Id { get; set; } = id;
}