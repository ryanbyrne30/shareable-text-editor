using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.TestAuth;

[ApiController]
public class TestAuthController
{
    private class Response
    {
        [JsonPropertyName("message")]
        public string Message { get; set; } = "Test auth successful";
    }
    
    [HttpGet("/api/v1/test-auth")]
    [Authorize]
    public IActionResult TestAuth()
    {
        return new OkObjectResult(new Response());
    }
}