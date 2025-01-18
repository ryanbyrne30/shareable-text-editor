using Microsoft.AspNetCore.Mvc;

namespace DocumentService.Users.Endpoints.VerifyUserPassword;

[ApiController]
public class VerifyUserPasswordController(VerifyUserPasswordService service): ControllerBase
{
    [HttpPost("/api/v1/users/validate-password")]
    public ActionResult<VerifyUserPasswordResponse> VerifyUserPassword([FromBody] VerifyUserPasswordRequest request)
    {
        service.VerifyUserPassword(request);
        var response = new VerifyUserPasswordResponse { Message = "Ok" };
        return Ok(response);
    }
    
}