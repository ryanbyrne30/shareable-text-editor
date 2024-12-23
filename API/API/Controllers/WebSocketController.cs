using System.Net.WebSockets;
using System.Text;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

public class WebSocketController: ControllerBase
{
    private static readonly List<WebSocket> Clients = []; 
    
    [Route("/ws")]
    public async Task Get()
    {
        if (HttpContext.WebSockets.IsWebSocketRequest)
        {
            var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
            Clients.Add(webSocket);
            await BroadcastClients();
            await Handler(webSocket);
        }
        else
        {
            HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
        }
    }

    private async Task BroadcastClients()
    {
        foreach (var client in Clients)
        {
            await client.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes($"Number of clients {Clients.Count}")), WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
    
    private async Task Handler(WebSocket webSocket)
    {
        var buffer = new byte[1024 * 4];
        var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        while (!result.CloseStatus.HasValue)
        {
            await Broadcast(result, buffer);
            result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
        }
        await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        Clients.Remove(webSocket);
    }
    
    private async Task Broadcast(WebSocketReceiveResult result, byte[] buffer)
    {
        foreach (var client in Clients)
        {
            await client.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
        }
    }
}