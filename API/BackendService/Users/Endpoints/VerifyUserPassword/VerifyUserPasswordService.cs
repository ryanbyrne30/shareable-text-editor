using System.Net;
using DocumentService.Common.Exceptions;
using DocumentService.Users.Repository;
using DocumentService.Users.Utils;

namespace DocumentService.Users.Endpoints.VerifyUserPassword;

public class VerifyUserPasswordService(UserRepository repository)
{
    public void VerifyUserPassword(VerifyUserPasswordRequest request)
    {
        var user = repository.Users.FirstOrDefault(u => u.Username == request.Username);
        if (user == null) throw new BadRequestException("User not found", HttpStatusCode.NotFound);
        var isVerified = CryptUtil.VerifyPassword(user, request.Password, user.PasswordHash);
        if (!isVerified) throw new BadRequestException("Invalid password", HttpStatusCode.Unauthorized);
    }
}