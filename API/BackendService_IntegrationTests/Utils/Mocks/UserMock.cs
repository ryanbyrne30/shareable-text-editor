using BackendService.Common;
using BackendService.Services.Users.Domain;
using BackendService.Services.Users.Utils;

namespace BackendService_IntegrationTests.Utils.Mocks;

public static class UserMock
{
    public const string Password = "password";
    
    public static User GenerateUser()
    {
        var user = new User
        {
            Id = DatabaseUtil.GenerateId(User.IdPrefix),
            Username = $"user_{GeneralMock.GenerateInt()}",
            PasswordHash = ""
        };

        var hash = CryptUtil.HashPassword(user, Password);
        user.PasswordHash = hash;

        return user;
    }
}