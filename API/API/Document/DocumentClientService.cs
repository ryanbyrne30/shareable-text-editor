using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using API.Document.Domain;

namespace API.Document;

public class DocumentClientService
{
    public async Task HandleClient(string docId, WebSocket webSocket)
    {
        var client = CreateClient(docId, webSocket);
        DocumentService.NewClient(docId, client);
        var result = await HandleMessages(docId, client);
        await webSocket.CloseAsync(result.CloseStatus ?? WebSocketCloseStatus.Empty, result.CloseStatusDescription, CancellationToken.None);
        DocumentService.RemoveClient(docId, client);
    }

    private static DocumentClient CreateClient(string docId, WebSocket webSocket)
    {
        return new DocumentClient
        {
            Id = Guid.NewGuid().ToString(),
            WebSocket = webSocket
        };
    }
    
    private static async Task<WebSocketReceiveResult> ReceiveAsync(WebSocket webSocket, byte[] buffer)
    { 
        return await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
    }

    private static DocumentAction? ParseMessage(string message)
    {
        try
        {
            return JsonSerializer.Deserialize<DocumentAction>(message);
        } catch (JsonException)
        {
            Console.WriteLine($"Invalid JSON: {message}");
            return null;
        }
    }

    private async Task<WebSocketReceiveResult> HandleMessages(string docId, DocumentClient client)
    {
        var buffer = new byte[1024 * 4];
        var result = await ReceiveAsync(client.WebSocket, buffer); 
        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var action = ParseMessage(message);
            if (action != null)
            {
                await HandleNewAction(docId, client, action);
            }
            result = await ReceiveAsync(client.WebSocket, buffer); 
        }

        return result;
    }

    private async Task HandleNewAction(string docId, DocumentClient client, DocumentAction action)
    {
            Console.WriteLine($"Received action: {action.ToString()}");
            var sendMessage = "Received action: " + action.ToString();
            var sendBytes = Encoding.UTF8.GetBytes(sendMessage);
            await client.WebSocket.SendAsync(new ArraySegment<byte>(sendBytes, 0, sendBytes.Length), WebSocketMessageType.Text,
                true, CancellationToken.None);
    }
}