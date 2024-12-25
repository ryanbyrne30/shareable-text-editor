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
}