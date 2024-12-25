using System.Net.WebSockets;

namespace API.Document.Domain;

public class DocumentClient
{
    public required string Id { get; init; }
    public required WebSocket WebSocket { get; init; }
}