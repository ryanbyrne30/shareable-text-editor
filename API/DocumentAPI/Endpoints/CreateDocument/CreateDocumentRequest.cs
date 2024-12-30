using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace DocumentAPI.Endpoints.CreateDocument;

public class CreateDocumentRequest
{
     [Required]
     [StringLength(100, ErrorMessage = "Title length can't be more than 100.")]
     [JsonPropertyName("title")]
     public required string Title { get; set; }
}