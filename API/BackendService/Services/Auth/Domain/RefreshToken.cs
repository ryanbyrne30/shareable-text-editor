using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Services.Auth.Domain;

[Index(nameof(Token), IsUnique = true)]
public class RefreshToken
{
    public const string IdPrefix = "rtok";
    
    [MaxLength(37)]
    public required string Id { get; set; }
    
    [MaxLength(37)]
    public required string UserId { get; set; }
    
    [MaxLength(48)]
    public required string Token { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    
    public DateTime ExpiresAt { get; set; }
}