using BackendService.Common.Exceptions;
using BackendService.Gateway.Utils;
using BackendService.Services.Users.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.GetAuthenticatedUser;

[ApiController]
public class GetAuthenticatedUserController(GetUserByUserIdService getUserByUserIdService): ControllerBase
{
    [HttpGet("/api/v1/users/me")]
    public async Task<ActionResult<GetAuthenticatedUserResponse>> GetAuthenticatedUser()
    {
        var userId = Authorize.GetCurrentUserId(HttpContext);
        if (userId == null) throw new UnauthenticatedRequestException();
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