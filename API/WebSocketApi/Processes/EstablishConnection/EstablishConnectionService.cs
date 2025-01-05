using System.Net.WebSockets;
using System.Text;
using WebSocketAPI.Services.DocumentService;
using WebSocketAPI.Services.DocumentService.CreateSession;
using WebSocketAPI.Services.DocumentService.SendMessage;
using WebSocketAPI.Stores;

namespace WebSocketAPI.Processes.EstablishConnection;

public class EstablishConnectionService(ILogger<EstablishConnectionService> logger, DocumentService documentService)
{
    public async Task HandleClient(string docId, WebSocket webSocket)
    {
        var socketId = WebSocketStore.AddWebSocket(webSocket);
        var sessionId = await CreateSessionId(docId, socketId); 
        if (sessionId == null)
        {
            await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Failed to create session", CancellationToken.None);
            return;
        }
        
        logger.LogInformation("Client connected to doc [{docId}] with socketId [{socketId}], session {sessionId}", docId, socketId, sessionId);
        var result = await HandleMessages(sessionId, webSocket);
        
        logger.LogInformation("Client [{socketId}] disconnected from doc [{docId}]: {CloseStatusDescription}", socketId, docId, result.CloseStatusDescription);
        await documentService.DeleteSession(sessionId);
    }
    
    private static async Task<WebSocketReceiveResult> ReceiveAsync(WebSocket webSocket, byte[] buffer)
    { 
        return await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    private async Task<string?> CreateSessionId(string docId, string socketId)
    {
        try
        {
            var response = await documentService.CreateSession(new CreateSessionRequest {
                 DocumentId = docId,
                 SocketId = socketId
             });
            return response.Id;
        } catch (HttpRequestException e)
        {
            logger.LogError(e, "Failed to create session for doc [{docId}] with socketId [{socketId}]", docId, socketId);
            return null;
        }
    }

    private async Task<WebSocketReceiveResult> HandleMessages(string sessionId, WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result = await ReceiveAsync(webSocket, buffer); 
        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            logger.LogInformation("Received message from client session [{sessionId}]: {message}", sessionId, message);
            await SendMessage(sessionId, message);
            result = await ReceiveAsync(webSocket, buffer); 
        }
        return result;
    }

    private async Task SendMessage(string sessionId, string message)
    {
        try
        {
            await documentService.SendMessage(sessionId, new SendMessageRequest { Message = message });
        } catch(Exception e)
        {
            logger.LogError(e, "Failed to send message to session [{sessionId}]", sessionId);
        }
    }
}