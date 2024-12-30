using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebSocketAPI.Stores;

namespace WebSocketAPI.Processes.SendMessage;

public class SendMessageController: ControllerBase
{
    [HttpPost("/sockets/socket/{socketId}")]
    public async Task Post(string socketId, [FromBody] string message)
    {
        var webSocket = WebSocketStore.GetWebSocket(socketId);
        if (webSocket != null)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }
}