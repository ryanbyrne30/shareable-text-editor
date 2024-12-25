namespace API.ChatRoom.Services;

public class ChatRoomService
{
    private static readonly Dictionary<string, Domain.ChatRoom> Chats = new();
    
    public static Domain.ChatRoom GetOrCreateChat(string chatId)
    {
        if (Chats.TryGetValue(chatId, out var chatRoom))
        {
            return chatRoom;
        }

        var newChat = new Domain.ChatRoom();
        Chats.Add(chatId, newChat);
        return newChat;
    }
}