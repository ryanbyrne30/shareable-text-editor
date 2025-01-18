using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Services.Users.Domain;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    public const string IdPrefix = "user";
    
    [MaxLength(37)]
    public required string Id { get; set; }
    
    [MaxLength(20)]
    public required string Username { get; set; }
    
    [MaxLength(100)]
    public required string PasswordHash { get; set; }
    
    [Required]
    public DateTime CreatedAt { get; set; } = DateTime.Now.ToUniversalTime();
    
    public DateTime? UpdatedAt { get; set; }
}