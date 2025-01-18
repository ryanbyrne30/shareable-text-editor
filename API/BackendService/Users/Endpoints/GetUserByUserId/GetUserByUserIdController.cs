using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Users.Endpoints.GetUserByUserId;

[ApiController]
public class GetUserByUserIdController(GetUserByUserIdService service): ControllerBase
{
    [HttpGet("/api/v1/users/{userId}")]
    public async Task<ActionResult<GetUserByUserIdResponse>> GetUserByUserId([FromRoute] [MaxLength(37)] string userId)
    {
        var user = await service.GetUserByUserId(userId);
        return Ok(user);
    }
}