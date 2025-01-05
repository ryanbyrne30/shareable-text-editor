using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Endpoints.CreateSession;

public class CreateSessionController(CreateSessionService service) : ControllerBase
{
    [HttpPost("/sessions")]
    public async Task<IActionResult> CreateSession([FromBody] CreateSessionRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }
        var sessionId = await service.CreateSession(request);
        var response = new CreateSessionResponse { Id = sessionId };
        return Ok(response);
    }
}