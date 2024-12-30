using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Domain;

public class Session
{
    public const string IdPrefix = "ses";
    
    [MaxLength(36)]
    public required string Id { get; set; }
    
    [MaxLength(36)]
    public required string DocumentId { get; set; }
    
    [MaxLength(36)]
    public required string SocketId { get; set; }
    
    public DateTime CreatedAt { get; set; }
}