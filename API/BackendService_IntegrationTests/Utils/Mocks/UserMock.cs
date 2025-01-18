using DocumentService.Common;
using DocumentService.Users.Domain;
using DocumentService.Users.Endpoints.GetUserByUserId;
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
            PasswordHash = ""
        };

        var hash = CryptUtil.HashPassword(user, Password);
        user.PasswordHash = hash;

        return user;
    }

    public static async Task<GetUserByUserIdResponse> FetchUserById(HttpClient client, string id)
    {
        var response = await client.GetAsync($"/api/v1/users/{id}");
        return RequestUtils.ParseResponse<GetUserByUserIdResponse>(response);
    }
}