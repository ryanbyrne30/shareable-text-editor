using DocumentService.Users.Domain;
using Microsoft.AspNetCore.Identity;

namespace DocumentService.Users.Utils;

public static class CryptUtil
{
    public static string HashPassword(User user, string password)
    {
        return new PasswordHasher<User>().HashPassword(user, password);
    }
    
    public static bool VerifyPassword(User user, string password, string hashedPassword)
    {
        return new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, password) == PasswordVerificationResult.Success;
    }
}