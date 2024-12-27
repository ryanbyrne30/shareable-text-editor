using System.Text.Json.Serialization;

namespace API.Document.Domain;

public class DocumentAction
{
    [JsonIgnore]
    public DocumentClient? Client { get; set; }
    
    [JsonPropertyName("revision")]
    public int Revision{ get; set; } 
    
    [JsonPropertyName("pos")]
    public int Position{ get; set; } 
    
    [JsonPropertyName("insert")]
    public string? Insert { get; set; } 
    
    [JsonPropertyName("delete")]
    public int? Delete { get; set; } 
    
    public new string ToString()
    {
        return $"Revision: {Revision}, Position: {Position}, Insert: {Insert}, Delete: {Delete}";
    }
    
    public bool IsInsert()
    {
        return Insert != null && Delete == null;
    }
    public bool IsDelete()
    {
        return Insert == null && Delete != null;
    }
    public bool IsUpdate()
    {
        return Insert != null && Delete != null;
    }
    
    public string Apply(string document)
    {
        if (Insert != null && Delete == null)
        {
            return document.Insert(Position, Insert);
        }
        if (Delete != null && Insert == null)
        {
            return document.Remove(Position, Delete.Value);
        }
        if (Insert != null && Delete != null)
        {
            return document.Remove(Position, Delete.Value).Insert(Position, Insert);
        }
        return document;
    }
}