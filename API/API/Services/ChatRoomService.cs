using API.Domain;

namespace API.Services;

public class ChatRoomService
{
    private static readonly Dictionary<string, ChatRoom> Chats = new();
    
    public static ChatRoom GetOrCreateChat(string chatId)
    {
        if (Chats.TryGetValue(chatId, out var chatRoom))
        {
            return chatRoom;
        }

        var newChat = new ChatRoom();
        Chats.Add(chatId, newChat);
        return newChat;
    }
}