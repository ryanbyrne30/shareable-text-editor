using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Users.Endpoints.CreateUser;

[ApiController]
public class CreateUserController(CreateUserService service): ControllerBase
{
    [HttpPost("/api/v1/users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var id = await service.CreateUser(request);
        var response = new CreateUserResponse(id);
        return Ok(response);
    }
}