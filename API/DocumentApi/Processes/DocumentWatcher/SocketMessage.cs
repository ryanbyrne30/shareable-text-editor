using System.Text.Json;
using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.DocumentWatcher;

public class SocketMessage
{
    [JsonPropertyName("ack")]
    public AckType? Ack { get; set; }
    
    [JsonPropertyName("action")]
    public ActionType? Action { get; set; }
    
    public class AckType
    {
        [JsonPropertyName("revision")]
        public int Revision { get; set; }
    }

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
    
    public static SocketMessage CreateAck(int revision) => new SocketMessage { Ack = new AckType { Revision = revision } };
    public static SocketMessage CreateAction(int revision, int position, string inserted, int deleted) => new SocketMessage
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