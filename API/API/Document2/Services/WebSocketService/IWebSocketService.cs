using System.Net.WebSockets;

namespace API.Document2.Services.WebSocketService;

public interface IWebSocketService
{
   Task Close(WebSocketCloseStatus status, string? description, CancellationToken cancellationToken);
   Task<WebSocketReceiveResult> Receive(byte[] buffer);
   Task Send(ArraySegment<byte> buffer, WebSocketMessageType messageType, bool endOfMessage, CancellationToken cancellationToken);
}