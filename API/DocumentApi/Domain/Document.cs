using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Domain;

public class Document(string id, string title = "", string content = "")
{
    public const string IdPrefix = "doc";

    [MaxLength(36)] [MinLength(36)] public string Id { get; set; } = id;

    [MaxLength(100)] public string Title { get; set; } = title;

    [MaxLength(100000)] public string Content { get; set; } = content; 

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}