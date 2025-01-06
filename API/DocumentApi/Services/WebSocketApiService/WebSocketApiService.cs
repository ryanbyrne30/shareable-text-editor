using DocumentAPI.Config;
using DocumentAPI.Services.WebSocketApiService.SendMessageToSocket;

namespace DocumentAPI.Services.WebSocketAPIService;

public class WebSocketApiService(HttpRequestService.HttpRequestService requestService, ILogger<WebSocketApiService> logger, AppConfig config) : IWebSocketApiService
{
    private string CreateUrl(string endpoint) => $"{config.WebSocketApiUrl}{endpoint}";

    public async Task SendMessageToSocket(string socketId, object message)
    {
        await requestService.Post<object, SendMessageToSocketResponse>(CreateUrl($"/sockets/socket/{socketId}"), message);
    }

}