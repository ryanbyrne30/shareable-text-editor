using BackendService.Gateway.Utils;
using BackendService.Services.Users.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.GetCurrentUser;

[ApiController]
public class GetCurrentUserController(GetUserByUserIdService getUserByUserIdService): ControllerBase
{
    public const string Endpoint = "/api/v1/users/me";
    
    [HttpGet(Endpoint)]
    [Authorize]
    public async Task<ActionResult<GetCurrentUserResponse>> GetAuthenticatedUser()
    {
        var userId = Authorize.GetRequiredCurrentUserId(HttpContext);
        var user = await getUserByUserIdService.GetUserByUserId(userId);
        return new GetCurrentUserResponse
        {
            Id = user.Id,
            Username = user.Username,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt
        };
    }
}