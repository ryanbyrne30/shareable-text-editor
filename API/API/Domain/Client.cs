using System.Net.WebSockets;
using System.Text;

namespace API.Domain;

public class Client
{
    public required string Id { get; init; }
    public required WebSocket WebSocket { get; init; }
    
    private static byte[] GetMessagePrefix(string sender, DateTime time) => Encoding.UTF8.GetBytes($"{time.Hour}:{time.Minute}:{time.Second} {sender}: ").ToArray();

    public async Task SendChat(string sender, byte[] buffer, WebSocketReceiveResult result)
    {
        var messageBytes = GetMessagePrefix(sender, DateTime.Now).Concat(new ArraySegment<byte>(buffer, 0, result.Count)).ToArray();
        await WebSocket.SendAsync(messageBytes, result.MessageType, result.EndOfMessage, CancellationToken.None);
    }

    public async Task SendMessage(ChatMessage message)
    {
        var messageBytes = GetMessagePrefix(message.Sender, message.Time).Concat(Encoding.UTF8.GetBytes(message.Message)).ToArray();
        await WebSocket.SendAsync(messageBytes, WebSocketMessageType.Text, true, CancellationToken.None);
    }
}