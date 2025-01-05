using System.Net.WebSockets;

namespace WebSocketAPI.Stores;

public static class WebSocketStore
{
    private static readonly Dictionary<string, WebSocket> WebSockets = new();
    
    public static WebSocket? GetWebSocket(string id)
    {
        return WebSockets.GetValueOrDefault(id); 
    }
    
    public static string AddWebSocket(WebSocket webSocket)
    {
        var id = Guid.NewGuid().ToString();
        WebSockets.Add(id, webSocket);
        return id;
    }
}