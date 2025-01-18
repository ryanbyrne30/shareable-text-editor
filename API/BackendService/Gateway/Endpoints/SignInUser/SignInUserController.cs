using BackendService.Services.Auth.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.SignInUser;

[ApiController]
public class SignInUserController(SignInUserService signInUserService): ControllerBase
{
    [HttpPost("/api/v1/auth/sign-in")]
    public ActionResult<SignInUserResponse> SignInUser([FromBody] SignInUserRequest request)
    {
        var response = signInUserService.SignInUser(request.Username, request.Password);
        return new SignInUserResponse
        {
            AuthToken = response.AuthToken,
            RefreshToken = response.RefreshToken
        };
    }
}