using System.Net.WebSockets;
using System.Text;

namespace API.ChatRoom.Domain;

public class ChatRoom
{
    private readonly List<Client> Clients = new();
    private readonly List<ChatMessage> Messages = new();

    private void AddMessage(string sender, string message)
    {
        var chatMessage = new ChatMessage
        {
            Sender = sender,
            Message = message,
            Time = DateTime.Now
        };
        Messages.Add(chatMessage);
    }

    public List<ChatMessage> GetMessages()
    {
        return Messages;
    }
    
    public async Task AddClient(Client client)
    {
        Clients.Add(client);
        foreach (var message in Messages)
        {
            await client.SendMessage(message);
        }
    }
    
    public void RemoveClient(Client client)
    {
        Clients.Remove(client);
    }
    
    public async Task Broadcast(Client client, byte[] buffer, WebSocketReceiveResult result)
    {
        var msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
        AddMessage(client.Id, msg);
        
        foreach (var c in Clients)
        {
            await c.SendChat(client.Id, buffer, result);
        }
    }
    
    public async Task BroadcastMessage(ChatMessage message)
    {
        AddMessage(message.Sender, message.Message);
        foreach (var client in Clients)
        {
            await client.SendMessage(message);
        }
    }
}