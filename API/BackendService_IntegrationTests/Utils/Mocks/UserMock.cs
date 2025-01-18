using DocumentService.Common;
using DocumentService.Users.Domain;
using DocumentService.Users.Utils;

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
            PasswordHash = "",
            CreatedAt = DateTime.Now.AddMinutes(-GeneralMock.GenerateInt(1, 3600))
        };

        var hash = CryptUtil.HashPassword(user, Password);
        user.PasswordHash = hash;

        return user;
    }
}