using DocumentService.Users.Domain;
using DocumentService.Users.Repository;

namespace DocumentService.Users.Endpoints.GetUserByUserId;

public class GetUserByUserIdService(UserRepository repo)
{
    public async Task<GetUserByUserIdResponse> GetUserByUserId(string userId)
    {
        var user = await repo.Users.FindAsync(userId);
        if (user == null) throw new BadHttpRequestException("User not found", 404);
        return GetUserByUserIdResponse.FromUser(user);
    }
    
}