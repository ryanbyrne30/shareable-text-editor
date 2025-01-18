using BackendService.Services.Users.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.CreateUser;

[ApiController]
public class CreateUserController(CreateUserService service): ControllerBase
{
    [HttpPost("/api/v1/users")]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var createUserRequest = new CreateUserService.Request(request.Username, request.Password);
        var id = await service.CreateUser(createUserRequest);
        var response = new CreateUserResponse(id);
        return Ok(response);
    }
}
