using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Processes.NewSessionMessage;

public class NewSessionMessageController(NewSessionMessageService service): ControllerBase
{
    [HttpPost("/sessions/session/{sessionId}")]
    public async Task Post(string sessionId, [FromBody] NewSessionMessageRequest request)
    {
        await service.HandleNewMessage(sessionId, request);
    }
}