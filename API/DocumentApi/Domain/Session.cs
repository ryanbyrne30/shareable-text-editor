using System.ComponentModel.DataAnnotations;

namespace DocumentAPI.Domain;

public class Session(string id, string documentId, string socketId)
{
    public const string IdPrefix = "ses";

    [MaxLength(36)] [MinLength(36)] public string Id { get; set; } = id;

    [MaxLength(36)] [MinLength(36)] public string DocumentId { get; set; } = documentId;

    [MaxLength(36)] [MinLength(36)] public string SocketId { get; set; } = socketId;

    public DateTime CreatedAt { get; set; } = DateTime.Now;
}