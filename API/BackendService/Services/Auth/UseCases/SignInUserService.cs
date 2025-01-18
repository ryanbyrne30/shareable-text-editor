using BackendService.Services.Auth.Utils;
using BackendService.Services.Users.UseCases;

namespace BackendService.Services.Auth.UseCases;

public class SignInUserService(VerifyUserPasswordService verifyUserPasswordService, GetUserByUsernameService getUserByUsernameService, TokenUtil tokenUtil)
{
    public sealed record Response(string AccessToken, string RefreshToken);
    
    public Response SignInUser(string username, string password)
    {
        verifyUserPasswordService.VerifyUserPassword(username, password);
        var user = getUserByUsernameService.GetUserByUsername(username);
        var accessToken = tokenUtil.CreateAccessToken(user.Id, user.Username);
        var refreshToken = tokenUtil.CreateRefreshToken();
        return new Response(accessToken, refreshToken);
    }
}