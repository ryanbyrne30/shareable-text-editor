using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Endpoints.NewSessionMessage;

public class NewSessionMessage
{
    [JsonPropertyName("new_action")]
    public NewActionType? NewAction { get; set; }

    public class NewActionType
    {
        [Required]
        [JsonPropertyName("revision")]
        [Range(1, int.MaxValue, ErrorMessage = "revision must be greater than 0")]
        public int Revision { get; set; }

        [Required]
        [JsonPropertyName("position")]
        [Range(0, int.MaxValue, ErrorMessage = "position must be greater than or equal to 0")]
        public int Position { get; set; }

        [Required]
        [JsonPropertyName("deleted")]
        [Range(0, int.MaxValue, ErrorMessage = "deleted must be greater than or equal to 0")]
        public int Deleted { get; set; }

        [Required]
        [StringLength(10000, ErrorMessage = "Inserted is too long.")]
        [JsonPropertyName("inserted")]
        public required string Inserted { get; set; }
    }

    public static NewSessionMessage CreateActionMessage(int revision, int position, string inserted, int deleted)
    {
        return new NewSessionMessage
        {
            NewAction = new NewActionType
            {
                Revision = revision,
                Position = position,
                Inserted = inserted,
                Deleted = deleted
            }
        };
    }
}