using BackendService.Services.Auth.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace BackendService.Gateway.Endpoints.RefreshUserTokens;

[ApiController]
public class RefreshUserTokensController(RefreshUserTokensService refreshUserTokensService): ControllerBase
{
    public const string Endpoint = "/api/v1/auth/refresh";
    
    [HttpPost(Endpoint)]
    public async Task<ActionResult<RefreshUserTokensResponse>> RefreshUserTokens([FromBody] RefreshUserTokensRequest request)
    {
        var result = await refreshUserTokensService.RefreshUserTokens(request.RefreshToken);
        return new RefreshUserTokensResponse
        {
            AccessToken = result.AccessToken,
            RefreshToken = result.RefreshToken
        };
    }
    
}