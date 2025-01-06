using System.Text.Json.Serialization;

namespace DocumentAPI.Endpoints.NewSessionMessage;

public class NewSessionMessageRequest
{
    [JsonPropertyName("action")]
    public required ActionType Action { get; set; }
    
    public class ActionType
    {
        [JsonPropertyName("revision")]
        public int Revision { get; set; }
        
        [JsonPropertyName("position")]
        public int Position { get; set; }
        
        [JsonPropertyName("inserted")]
        public required string Inserted { get; set; }
        
        [JsonPropertyName("deleted")]
        public int Deleted { get; set; }
    }

    public static NewSessionMessageRequest CreateActionMessage(int revision, int position, string inserted, int deleted)
    {
        return new NewSessionMessageRequest
        {
            Action = new ActionType
            {
                Revision = revision,
                Position = position,
                Inserted = inserted,
                Deleted = deleted
            }
        };
    }
}