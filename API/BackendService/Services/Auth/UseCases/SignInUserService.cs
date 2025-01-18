using BackendService.Services.Users.UseCases;

namespace BackendService.Services.Auth.UseCases;

public class SignInUserService(VerifyUserPasswordService verifyUserPasswordService, GetUserByUsernameService getUserByUsernameService)
{
    public sealed record Response(string AuthToken, string RefreshToken);
    
    public Response SignInUser(string username, string password)
    {
        verifyUserPasswordService.VerifyUserPassword(username, password);
        var user = getUserByUsernameService.GetUserByUsername(username);
        return new Response("auth", "refresh");
    }
}