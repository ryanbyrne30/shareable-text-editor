using System.Net.WebSockets;

namespace API.Document2.Services.WebSocketService;

public class WebSocketService(WebSocket webSocket) : IWebSocketService
{
    public Task Close(WebSocketCloseStatus status, string? description, CancellationToken cancellationToken)
    {
        return webSocket.CloseAsync(status, description, cancellationToken);
    }
    
    public Task<WebSocketReceiveResult> Receive(byte[] buffer)
    {
        return webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }
    
    public Task Send(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken)
    {
        return webSocket.SendAsync(buffer, messageType, endOfMessage, cancellationToken);
    }
}