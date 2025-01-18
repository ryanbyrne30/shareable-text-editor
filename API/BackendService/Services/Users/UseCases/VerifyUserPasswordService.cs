using System.Net;
using BackendService.Common.Exceptions;
using BackendService.Services.Users.Repository;
using BackendService.Services.Users.Utils;

namespace BackendService.Services.Users.UseCases;

public class VerifyUserPasswordService(UserRepository repository)
{
    public void VerifyUserPassword(string username, string password)
    {
        var user = repository.Users.FirstOrDefault(u => u.Username == username);
        if (user == null) throw new BadRequestException("User not found", HttpStatusCode.NotFound);
        var isVerified = CryptUtil.VerifyPassword(user, password, user.PasswordHash);
        if (!isVerified) throw new BadRequestException("Invalid password", HttpStatusCode.Unauthorized);
    }
}