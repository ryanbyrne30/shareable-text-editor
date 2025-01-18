using DocumentService.Users.Domain;
using Microsoft.AspNetCore.Identity;

namespace DocumentService.Users.Utils;

public static class CryptUtil
{
    public static string HashPassword(User user, string password)
    {
        return new PasswordHasher<User>().HashPassword(user, password);
    }
}