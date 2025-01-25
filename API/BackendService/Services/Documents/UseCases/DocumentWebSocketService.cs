using System.Net.WebSockets;
using BackendService.Common.Repositories;
using BackendService.Services.Documents.Services;

namespace BackendService.Services.Documents.UseCases;

public class DocumentWebSocketService(AppRepository repository)
{
    public async Task HandleClient(string documentId, WebSocket webSocket)
    {
        var clientId = DocumentWebSocketStoreService.AddWebSocket(documentId, webSocket);
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        try
        {
            while (!result.CloseStatus.HasValue)
            {
                var messageBytes = new ArraySegment<byte>(buffer, 0, result.Count).ToArray();
                
                await DocumentEventStore.ApplyEvent(messageBytes, documentId, repository);
                
                var clients = DocumentWebSocketStoreService.GetClients(documentId);
                foreach (var client in clients)
                {
                    if (client.Id == clientId) continue;
                    if (client.Socket.State == WebSocketState.Open) {
                        await client.Socket.SendAsync(messageBytes, result.MessageType, result.EndOfMessage, CancellationToken.None);
                    }
                }

                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
        }
        finally
        {
            await webSocket.CloseAsync(result.CloseStatus ?? WebSocketCloseStatus.NormalClosure, result.CloseStatusDescription, CancellationToken.None);
            DocumentWebSocketStoreService.RemoveClient(documentId, clientId);
        }
    }
}