using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using API.Document.Domain;

namespace API.Document;

public class DocumentClientService
{
    public async Task HandleClient(string docId, WebSocket webSocket)
    {
        var client = CreateClient(webSocket);
        Console.WriteLine($"Doc [{docId}]: New client {client.Id}");
        DocumentService.NewClient(docId, client);
        var result = await HandleMessages(docId, client);
        await webSocket.CloseAsync(result.CloseStatus ?? WebSocketCloseStatus.Empty, result.CloseStatusDescription, CancellationToken.None);
        DocumentService.RemoveClient(docId, client);
    }

    private static DocumentClient CreateClient(WebSocket webSocket)
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
                action.Client = client;
                Console.WriteLine($"Received action from client [{client.Id}]: {action.ToString()}");
                DocumentService.NewAction(docId, action);
            }
            result = await ReceiveAsync(client.WebSocket, buffer); 
        }

        return result;
    }
    
    public static async Task BroadcastAction(string docId, DocumentAction action)
    {
        var clients = DocumentService.GetDocumentClients(docId);
        foreach (var client in clients)
        {
            if (client.WebSocket.State == WebSocketState.Closed) continue;
            if (client.Id == action.Client?.Id) await AcknowledgeClient(docId, client);
            else await SendAction(client, action);
        }
    }

    private static async Task AcknowledgeClient(string docId, DocumentClient client)
    {
        Console.WriteLine($"Acknowledging client {client.Id}");
        var response = new DocumentResponse
        {
            Ack = new DocumentResponse.Acknowledgement
            {
                Success = true,
                Version = DocumentService.GetDocumentVersion(docId)
            }
        };
        var json = JsonSerializer.Serialize(response);
        var bytes = Encoding.UTF8.GetBytes(json);
        await client.WebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }
    
    private static async Task SendAction(DocumentClient client, DocumentAction action)
    {
        Console.WriteLine($"Sending action to {client.Id}");
        var json = JsonSerializer.Serialize(action);
        var bytes = Encoding.UTF8.GetBytes(json);
        await client.WebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
    }
}