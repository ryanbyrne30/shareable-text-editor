using BackendService.Common;
using BackendService.Common.Repositories;
using BackendService.Services.Auth.Domain;
using BackendService.Services.Auth.Utils;
using BackendService.Services.Users.UseCases;

namespace BackendService.Services.Auth.UseCases;

public class SignInUserService(VerifyUserPasswordService verifyUserPasswordService, GetUserByUsernameService getUserByUsernameService, TokenUtil tokenUtil, AppRepository appRepository)
{
    public sealed record Response(string AccessToken, string RefreshToken);
    
    public async Task<Response> SignInUser(string username, string password)
    {
        verifyUserPasswordService.VerifyUserPassword(username, password);
        var user = getUserByUsernameService.GetUserByUsername(username);
        var accessToken = tokenUtil.CreateAccessToken(user.Id, user.Username);
        var refreshToken = tokenUtil.CreateRefreshToken();

        var refreshTokenEntity = new RefreshToken
        {
            Id = DatabaseUtil.GenerateId(RefreshToken.IdPrefix),
            Token = refreshToken,
            UserId = user.Id,
            ExpiresAt = tokenUtil.RefreshTokenExpirationMinutes()
        };
        appRepository.Add(refreshTokenEntity);
        await appRepository.SaveChangesAsync();
        
        return new Response(accessToken, refreshToken);
    }
}