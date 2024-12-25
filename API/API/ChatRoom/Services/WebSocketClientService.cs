using System.Net.WebSockets;
using API.ChatRoom.Domain;

namespace API.ChatRoom.Services;

public class WebSocketClientService
{
    
    public async Task HandleClient(string chatId, WebSocket webSocket)
    {
        var chatRoom = ChatRoomService.GetOrCreateChat(chatId); 
        var client = new Client
        {
            Id = Guid.NewGuid().ToString()[..6],
            WebSocket = webSocket
        };
        
        await chatRoom.AddClient(client);
        await chatRoom.BroadcastMessage(new ChatMessage
        {
            Message = $"New user joined the chat: {client.Id}",
            Sender = "System",
            Time = DateTime.Now
        });
        
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            await chatRoom.Broadcast(client, buffer, result);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        chatRoom.RemoveClient(client);
    }
    
}