using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using API.Document.Domain;

namespace API.Document;

public class DocumentClientService
{
    public async Task HandleClient(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
            var action = ParseMessage(message);
            if (action == null)
            {
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                continue;
            }
            
            Console.WriteLine($"Received action: {action.ToString()}");
            var sendMessage = "Received action: " + action.ToString();
            var sendBytes = Encoding.UTF8.GetBytes(sendMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(sendBytes, 0, sendBytes.Length), WebSocketMessageType.Text,
                true, CancellationToken.None);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
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
}