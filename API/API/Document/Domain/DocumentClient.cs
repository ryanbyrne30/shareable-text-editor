using System.Net.WebSockets;

namespace API.Document.Domain;

public class DocumentClient
{
    public required string Id { get; init; }
    public required System.Net.WebSockets.WebSocket WebSocket { get; init; }
}