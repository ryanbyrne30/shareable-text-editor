using System.Net.WebSockets;

namespace BackendService.Services.Documents.Services;

public static class DocumentWebSocketStoreService
{
    public sealed record Client(string Id, WebSocket Socket);
    
    private static readonly Dictionary<string, List<Client>> Clients = new();
    
    public static string AddWebSocket(string documentId, WebSocket webSocket)
    {
        var id = Guid.NewGuid().ToString();
        var client = new Client(id, webSocket);
        
        if (Clients.TryGetValue(documentId, out var webSockets))
        {
            webSockets.Add(client);
        }
        else
        {
            Clients.Add(documentId, [client]);
        }

        return id;
    }
    
    public static List<Client> GetClients(string documentId)
    {
        return Clients.TryGetValue(documentId, out var clients) ? clients : [];
    }
    
    public static void RemoveClient(string documentId, string clientId)
    {
        if (Clients.TryGetValue(documentId, out var clients))
        {
            clients.RemoveAll(c => c.Id == clientId);
        }
    }
}