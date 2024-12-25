using API.ChatRoom.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Document; 

public class DocumentWebSocketController(DocumentClientService clientService): ControllerBase
{
    [Route("/doc")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await clientService.HandleClient(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}