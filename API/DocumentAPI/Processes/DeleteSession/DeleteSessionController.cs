using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Processes.DeleteSession;

public class DeleteSessionController(DeleteSessionService service): ControllerBase
{
    [HttpDelete("/sessions/session/{sessionId}")]
    public async Task<IActionResult> Delete(string sessionId)
    {
        await service.DeleteSessions(sessionId);
        var response = new DeleteSessionResponse
        {
            Message = "Sessions deleted"
        };
        return Ok(response);
    }
    
}