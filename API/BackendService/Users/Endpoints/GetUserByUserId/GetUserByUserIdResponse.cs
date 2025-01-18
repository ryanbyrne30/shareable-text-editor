using System.Text.Json.Serialization;
using DocumentService.Users.Domain;

namespace DocumentService.Users.Endpoints.GetUserByUserId;

public class GetUserByUserIdResponse
{
    [JsonPropertyName("id")]
    public required string Id { get; set; } 
    
    [JsonPropertyName("username")]
    public required string Username { get; set; } 
    
    [JsonPropertyName("createdAt")]
    public DateTime CreatedAt { get; set; } 
    
    [JsonPropertyName("updatedAt")]
    public DateTime? UpdatedAt { get; set; } 
    
    public static GetUserByUserIdResponse FromUser(User user) => new() 
    {
        Id = user.Id,
        Username = user.Username,
        CreatedAt = user.CreatedAt,
        UpdatedAt = user.UpdatedAt
    };
}