using API.ChatRoom.Services;
using Microsoft.AspNetCore.Mvc;

namespace API.Document; 

public class DocumentWebSocketController(DocumentClientService clientService): ControllerBase
{
    [Route("/doc/{docId}")]
    public async Task Get(string docId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await clientService.HandleClient(docId, webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}