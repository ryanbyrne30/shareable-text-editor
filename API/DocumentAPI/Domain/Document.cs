using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Domain;

public class Document
{
    public const string IdPrefix = "doc";
    
    [MaxLength(36)]
    [MinLength(36)]
    public required string Id { get; set; }
    
    [MaxLength(100)]
    public required string Title { get; set; }
    
    [MaxLength(100000)]
    public required string Content { get; set; }
    
    public DateTime CreatedAt { get; set; }
}