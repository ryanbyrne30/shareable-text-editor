using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Users.Endpoints.CreateUser;

[ApiController]
public class CreateUserController(CreateUserService service): ControllerBase
{
    [HttpPost("/api/v1/users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var id = await service.CreateUser(request);
        var response = new CreateUserResponse(id);
        return Ok(response);
    }
}