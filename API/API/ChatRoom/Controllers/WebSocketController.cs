using API.ChatRoom.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.ChatRoom.Controllers;

public class WebSocketController(WebSocketClientService clientService): ControllerBase
{
    [Route("/ws/{chatId}")]
    public async Task Get(string chatId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await clientService.HandleClient(chatId, webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}