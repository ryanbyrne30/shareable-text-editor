using BackendService.Gateway.Utils;
using BackendService.Services.Users.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.GetAuthenticatedUser;

[ApiController]
public class GetAuthenticatedUserController(GetUserByUserIdService getUserByUserIdService): ControllerBase
{
    public const string Endpoint = "/api/v1/users/me";
    
    [HttpGet(Endpoint)]
    public async Task<ActionResult<GetAuthenticatedUserResponse>> GetAuthenticatedUser()
    {
        var userId = Authorize.GetRequiredCurrentUserId(HttpContext);
        var user = await getUserByUserIdService.GetUserByUserId(userId);
        return new GetAuthenticatedUserResponse
        {
            Id = user.Id,
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}