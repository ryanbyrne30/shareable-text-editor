using System.Net.WebSockets;
using BackendService.Services.Documents.Services;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.DocumentWebSocketP2P;


public class DocumentWebSocketP2PController: ControllerBase
{
    public const string Endpoint = "/ws/v1/documents/{id}/p2p";

    [Route(Endpoint)]
    public async Task Connect(string id)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await HandleClient(id, webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    public async Task HandleClient(string documentId, WebSocket webSocket)
    {
        var docId = documentId + "-p2p";
        var clientId = DocumentWebSocketStoreService.AddWebSocket(docId, webSocket);
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

        try
        {
            while (!result.CloseStatus.HasValue)
            {
                var messageBytes = new ArraySegment<byte>(buffer, 0, result.Count).ToArray();
                
                var clients = DocumentWebSocketStoreService.GetClients(docId);
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
            DocumentWebSocketStoreService.RemoveClient(docId, clientId);
        }
    }
}