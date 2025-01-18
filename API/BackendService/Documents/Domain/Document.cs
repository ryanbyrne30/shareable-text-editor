namespace DocumentService.Documents.Domain;

public class Document
{
    public const string IdPrefix = "doc";
    
    public required string Id { get; set; }
    
}