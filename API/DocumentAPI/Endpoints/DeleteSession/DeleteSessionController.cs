using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Endpoints.DeleteSession;

public class DeleteSessionController(DeleteSessionService service): ControllerBase
{
    [HttpDelete("/sessions/session/{sessionId}")]
    public async Task<IActionResult> Delete(string sessionId)
    {
        await service.DeleteSession(sessionId);
        var response = new DeleteSessionResponse
        {
            Message = "Session deleted"
        };
        return Ok(response);
    }
    
}