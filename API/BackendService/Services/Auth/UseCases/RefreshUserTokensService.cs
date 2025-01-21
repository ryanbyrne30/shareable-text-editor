using BackendService.Common.Exceptions;
using BackendService.Common.Repositories;
using BackendService.Services.Auth.Utils;
using BackendService.Services.Users.UseCases;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Services.Auth.UseCases;

public class RefreshUserTokensService(TokenUtil tokenUtil, AppRepository appRepository, GetUserByUserIdService getUserByUserIdService)
{
    public sealed record Response(string AccessToken, string RefreshToken);
    
    public async Task<Response> RefreshUserTokens(string refreshToken)
    {
        var refreshTokenEntity = await appRepository.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);
        if (refreshTokenEntity is null) throw new UnauthenticatedRequestException();
        
        var user = await getUserByUserIdService.GetUserByUserId(refreshTokenEntity.UserId);
        
        var accessToken = tokenUtil.CreateAccessToken(user.Id, user.Username);
        var newRefreshToken = tokenUtil.CreateRefreshToken();

        refreshTokenEntity.Token = newRefreshToken;
        refreshTokenEntity.ExpiresAt = tokenUtil.RefreshTokenExpirationMinutes();
        appRepository.Update(refreshTokenEntity);
        await appRepository.SaveChangesAsync();
        
        return new Response(accessToken, newRefreshToken);
    }
    
}