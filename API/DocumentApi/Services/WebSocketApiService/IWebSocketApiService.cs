namespace DocumentAPI.Services.WebSocketAPIService;

public interface IWebSocketApiService
{
    public Task SendMessageToSocket(string socketId, object message);
}