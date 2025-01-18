using System.Net;
using BackendService.Common.Exceptions;
using BackendService.Services.Users.Repository;

namespace BackendService.Services.Users.UseCases;

public class GetUserByUsernameService(UserRepository userRepository)
{
    public sealed record Response(string Id, string Username, DateTime CreatedAt, DateTime? UpdatedAt);
    
    public Response GetUserByUsername(string username)
    {
        var user = userRepository.Users.FirstOrDefault(u => u.Username == username);
        if (user is null) throw new BadRequestException("User not found", HttpStatusCode.NotFound);
        return new Response(user.Id, user.Username, user.CreatedAt, user.UpdatedAt);
    }
    
}