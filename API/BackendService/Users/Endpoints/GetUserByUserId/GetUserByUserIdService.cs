using System.Net;
using DocumentService.Common.Exceptions;
using DocumentService.Users.Domain;
using DocumentService.Users.Repository;

namespace DocumentService.Users.Endpoints.GetUserByUserId;

public class GetUserByUserIdService(UserRepository repo)
{
    public async Task<GetUserByUserIdResponse> GetUserByUserId(string userId)
    {
        var user = await repo.Users.FindAsync(userId);
        if (user == null) throw new BadRequestException("User not found", HttpStatusCode.NotFound);
        return GetUserByUserIdResponse.FromUser(user);
    }
    
}