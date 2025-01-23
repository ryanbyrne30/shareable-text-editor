using System.ComponentModel.DataAnnotations;

namespace BackendService.Services.Documents.Domain;

public class Document
{
    public const string IdPrefix = "doc";
    
    [Key]
    [MaxLength(36)]
    public required string Id { get; init; }
    
    [MaxLength(37)]
    public required string UserId { get; init; }
    
    [MaxLength(100)]
    public required string Name { get; set; }
}