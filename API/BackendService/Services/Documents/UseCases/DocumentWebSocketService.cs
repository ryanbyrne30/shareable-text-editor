using System.Net.WebSockets;
using BackendService.Common.Repositories;

namespace BackendService.Services.Documents.UseCases;

public class DocumentWebSocketService(AppRepository repository)
{
    public async Task HandleClient(string documentId, WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            var messageBytes = new ArraySegment<byte>(buffer, 0, result.Count).ToArray();
            await webSocket.SendAsync(messageBytes, result.MessageType, result.EndOfMessage, CancellationToken.None);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
    }
}