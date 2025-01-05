using Microsoft.AspNetCore.Mvc;

namespace DocumentAPI.Endpoints.Health;

public class HealthController : ControllerBase
{
    [HttpGet("/health")]
    public IActionResult Get()
    {
        return Ok();
    }

}