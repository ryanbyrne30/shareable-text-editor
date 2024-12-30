using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Processes.DeleteSocketSessions;

public class DeleteSocketSessionsController(DeleteSocketSessionsService service): ControllerBase
{
    [HttpDelete("/sockets/socket/{sessionId}")]
    public async Task<IActionResult> Delete(string sessionId)
    {
        await service.DeleteSessions(sessionId);
        var response = new DeleteSocketSessionsResponse
        {
            Message = "Sessions deleted"
        };
        return Ok(response);
    }
    
}