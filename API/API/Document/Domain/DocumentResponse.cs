using System.Text.Json.Serialization;

namespace API.Document.Domain;

public class DocumentResponse
{
    [JsonPropertyName("ack")]
    public Acknowledgement Ack { get; set; }
    
    public class Acknowledgement
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        
        [JsonPropertyName("version")]
        public int Version { get; set; }
    }
}