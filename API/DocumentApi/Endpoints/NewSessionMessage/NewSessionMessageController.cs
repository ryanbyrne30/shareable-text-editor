using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Endpoints.NewSessionMessage;

public class NewSessionMessageController(NewSessionMessageService service) : ControllerBase
{
    [HttpPost("/sessions/session/{sessionId}")]
    public async Task<IActionResult> Post(string sessionId, [FromBody] NewSessionMessageRequest request)
    {
        await service.HandleNewMessage(sessionId, request);
        var response = new NewSessionMessageResponse { Message = "Message received" };
        return Ok(response);
    }
}