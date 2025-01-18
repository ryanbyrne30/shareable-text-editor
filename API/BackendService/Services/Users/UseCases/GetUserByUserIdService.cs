using System.Net;
using BackendService.Common.Exceptions;
using BackendService.Services.Users.Repository;

namespace BackendService.Services.Users.UseCases;

public class GetUserByUserIdService(UserRepository repo)
{
    public sealed record Response(string Id, string Username, DateTime CreatedAt, DateTime? UpdatedAt);
    
    public async Task<Response> GetUserByUserId(string userId)
    {
        var user = await repo.Users.FindAsync(userId);
        if (user == null) throw new BadRequestException("User not found", HttpStatusCode.NotFound);
        return new Response(user.Id, user.Username, user.CreatedAt, user.UpdatedAt);
    }
}