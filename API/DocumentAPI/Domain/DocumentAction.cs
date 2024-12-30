using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Domain;

public class DocumentAction
{
    public const string IdPrefix = "act";
    
    [MaxLength(36)]
    [MinLength(36)]
    public required string Id { get; set; }
    
    [MaxLength(36)]
    [MinLength(36)]
    public required string SessionId { get; set; }
    
    [MaxLength(36)]
    [MinLength(36)]
    public required string DocumentId { get; set; }
    
    public DateTime OccurredAt { get; set; }
    public DateTime? CompletedAt { get; set; } = null;
    public long Revision { get; set; }
    public ulong Position { get; set; }
    public ulong Deleted { get; set; }
    
    [MaxLength(10000)]
    public required string Inserted { get; set; }
}