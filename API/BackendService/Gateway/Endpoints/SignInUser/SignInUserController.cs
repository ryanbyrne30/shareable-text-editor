using BackendService.Services.Auth.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.SignInUser;

[ApiController]
public class SignInUserController(SignInUserService signInUserService): ControllerBase
{
    public const string Endpoint = "/api/v1/auth/sign-in";
    
    [HttpPost(Endpoint)]
    public async Task<ActionResult<SignInUserResponse>> SignInUser([FromBody] SignInUserRequest request)
    {
        var response = await signInUserService.SignInUser(request.Username, request.Password);
        return new SignInUserResponse
        {
            AccessToken = response.AccessToken,
            RefreshToken = response.RefreshToken
        };
    }
}