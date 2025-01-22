using System.Net;
using BackendService.Common.Exceptions;

namespace BackendService.Gateway.Utils;

public static class Authorize
{
    private const string IdClaimType = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier";
    
    public static string? GetCurrentUserId(HttpContext context)
    {
        return context.User.Claims.FirstOrDefault(c => c.Type == IdClaimType)?.Value;
    }
    
    public static string GetRequiredCurrentUserId(HttpContext context)
    {
        var userId = context.User.Claims.FirstOrDefault(c => c.Type == IdClaimType)?.Value;
        if (userId == null) throw new UnauthorizedRequestException();
        return userId;
    }
    
    public static void AuthorizeUserId(HttpContext context, string id)
    {
        var tokenUserId = GetCurrentUserId(context);
        if (tokenUserId != id) throw new BadRequestException("Unauthorized", HttpStatusCode.Unauthorized);
    } 
}