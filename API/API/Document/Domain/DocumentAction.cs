using System.Text.Json.Serialization;

namespace API.Document.Domain;

public class DocumentAction(int revision, int position, string? insert, int? delete, DocumentClient? client = null)
{

    [JsonIgnore] public DocumentClient? Client { get; set; } = client;
    [JsonPropertyName("revision")] public int Revision { get; set; } = revision;
    [JsonPropertyName("pos")] public int Position { get; set; } = position;
    [JsonPropertyName("insert")] public string? Insert { get; set; } = insert;
    [JsonPropertyName("delete")] public int? Delete { get; set; } = delete;

    public new string ToString() => $"Revision: {Revision}, Position: {Position}, Insert: {Insert}, Delete: {Delete}";
    public bool IsInsert() => Insert != null && Delete == null;
    public bool IsDelete() => Insert == null && Delete != null;
    public bool IsUpdate() => Insert != null && Delete != null;
    
    public string Apply(string document)
    {
        if (Insert != null && Delete == null) return document.Insert(Position, Insert);
        if (Delete != null && Insert == null) return document.Remove(Position, Delete.Value);
        if (Insert != null && Delete != null) return document.Remove(Position, Delete.Value).Insert(Position, Insert);
        return document;
    }
}