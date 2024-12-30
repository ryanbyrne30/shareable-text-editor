using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Processes.NewSessionMessage;

public class NewSessionMessage
{
    [JsonPropertyName("new_action")]
    public NewActionType? NewAction { get; set; }
    
    public class NewActionType 
    {
        [Required]
        [JsonPropertyName("revision")]
        [Range(1, ulong.MaxValue, ErrorMessage = "revision must be greater than 0")]
        public ulong Revision { get; set; }
        
        [Required]
        [JsonPropertyName("position")]
        [Range(0, ulong.MaxValue, ErrorMessage = "position must be greater than or equal to 0")]
        public ulong Position { get; set; }
        
        [Required]
        [JsonPropertyName("deleted")]
        [Range(0, ulong.MaxValue, ErrorMessage = "deleted must be greater than or equal to 0")]
        public ulong Deleted { get; set; }
        
        [Required]
        [StringLength(10000, ErrorMessage = "Inserted is too long.")]
        [JsonPropertyName("inserted")]
        public required string Inserted { get; set; }
    }
}