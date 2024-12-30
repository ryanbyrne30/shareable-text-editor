using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using WebSocketAPI.Stores;

namespace WebSocketAPI.Processes.SendMessage;

public class SendMessageController: ControllerBase
{
    [HttpPost("/sockets/socket/{socketId}")]
    public async Task<IActionResult> Post(string socketId, [FromBody] SendMessageRequest request)
    {
        var webSocket = WebSocketStore.GetWebSocket(socketId);
        if (webSocket != null)
        {
            var buffer = Encoding.UTF8.GetBytes(request.Message);
            await webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            var response = new SendMessageResponse{Message = "Message sent"};
            return Ok(response);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return BadRequest("Socket not found");
        }
    }
}