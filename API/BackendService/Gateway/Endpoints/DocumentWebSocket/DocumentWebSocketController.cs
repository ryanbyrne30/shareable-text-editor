using BackendService.Services.Documents.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.DocumentWebSocket;


public class DocumentWebSocketController(DocumentWebSocketService documentWebSocketService): ControllerBase
{
    public const string Endpoint = "/ws/v1/documents/{id}";

    [Route(Endpoint)]
    public async Task Connect(string id)
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            await documentWebSocketService.HandleClient(id, webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

}