using BackendService.Services.Users.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.RegisterUser;

[ApiController]
public class RegisterUserController(CreateUserService service): ControllerBase
{
    [HttpPost("/api/v1/users")]
    public async Task<IActionResult> CreateUser([FromBody] RegisterUserRequest request)
    {
        var createUserRequest = new CreateUserService.Request(request.Username, request.Password);
        var id = await service.CreateUser(createUserRequest);
        var response = new RegisterUserResponse(id);
        return Ok(response);
    }
}
