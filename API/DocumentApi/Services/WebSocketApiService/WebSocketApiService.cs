using DocumentAPI.Config;
using DocumentAPI.Services.WebSocketApiService.SendMessageToSocket;

namespace DocumentAPI.Services.WebSocketAPIService;

public class WebSocketApiService(HttpRequestService.HttpRequestService requestService, ILogger<WebSocketApiService> logger, AppConfig config) : IWebSocketApiService
{
    private string CreateUrl(string endpoint) => $"{config.WebSocketApiUrl}{endpoint}";

    public async Task SendMessageToSocket(string socketId, string message)
    {
        await requestService.Post<SendMessageToSocketRequest, SendMessageToSocketResponse>(CreateUrl($"/sockets/socket/{socketId}"), new SendMessageToSocketRequest { Message = message });
    }

}