using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils.Mocks;
using BackendService.Gateway.Endpoints.SignInUser;
using BackendService.Services.Users.Domain;

namespace BackendService_IntegrationTests.Utils;

public class UserUtils
{
    public static User CreateUser(CustomWebApplicationFactory factory)
    {
        var user = UserMock.GenerateUser();
        factory.SeedUserData(context =>
        {
            context.Users.Add(user);
            context.SaveChanges();
        });
        return user;
    }
    
    public static async Task<SignInUserResponse> SignInUser(HttpClient client, User user)
    {
        var request = new
        {
          username = user.Username,
          password = UserMock.Password 
        };

        var signInResponse = await client.PostAsJsonAsync(SignInUserController.Endpoint, request);
        Assert.That(signInResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var response = RequestUtils.ParseResponse<SignInUserResponse>(signInResponse);
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", response.AccessToken);
        return response;
    }
}