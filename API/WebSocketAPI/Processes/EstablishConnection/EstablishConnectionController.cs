using Microsoft.AspNetCore.Mvc;

namespace WebSocketAPI.Processes.EstablishConnection;

public class EstablishConnectionController(EstablishConnectionService service): ControllerBase
{
    [ApiExplorerSettings(IgnoreApi = true)]
    [Route("/ws/docs/doc/{docId}")]
    public async Task Get(string docId)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await service.HandleClient(docId, webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
    
}