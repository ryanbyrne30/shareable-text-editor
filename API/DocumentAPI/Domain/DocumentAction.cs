using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Domain;

public class DocumentAction
{
    public const string IdPrefix = "act";
    
    [MaxLength(36)]
    public required string Id { get; set; }
    
    [MaxLength(36)]
    public required string SessionId { get; set; }
    
    public DateTime OccurredAt { get; set; }
    public bool IsCompleted { get; set; }
    public long Revision { get; set; }
    public long Position { get; set; }
    public long Deleted { get; set; }
    
    [MaxLength(10000)]
    public required string Inserted { get; set; }
}