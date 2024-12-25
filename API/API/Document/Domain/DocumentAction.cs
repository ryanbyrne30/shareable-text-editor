using System.Text.Json.Serialization;

namespace API.Document.Domain;

public class DocumentAction
{
    [JsonPropertyName("a")]
    public int RetainAfter { get; set; } 
    
    [JsonPropertyName("b")]
    public int RetainBefore { get; set; } 
    
    [JsonPropertyName("i")]
    public string? Insert { get; set; } 
    
    [JsonPropertyName("d")]
    public int? Delete { get; set; } 
    
    public string ToString() => $"[RetainAfter: {RetainAfter}, RetainBefore: {RetainBefore}, Insert: {Insert}, Delete: {Delete}]";
}