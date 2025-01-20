using System.ComponentModel.DataAnnotations;
using BackendService.Gateway.Utils;
using BackendService.Services.Users.UseCases;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.GetUserById;

[ApiController]
public class GetUserByIdController(GetUserByUserIdService getUserByUserIdService): ControllerBase
{
    [HttpGet("/api/v1/users/{id}")]
    [Authorize]
    public async Task<ActionResult<GetUserByIdResponse>> GetUserById([MaxLength(100)] string id)
    {
        Authorize.AuthorizeUserId(HttpContext, id);
        var user = await getUserByUserIdService.GetUserByUserId(id);
        return new GetUserByIdResponse
        {
            Id = user.Id,
            Username = user.Username
        };
    }
}