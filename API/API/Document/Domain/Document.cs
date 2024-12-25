namespace API.Document.Domain;

public class Document
{
    public required string Id { get; set; }
    public string Content { get; set; } = "";
    public List<DocumentClient> Clients { get; set; } = [];
}