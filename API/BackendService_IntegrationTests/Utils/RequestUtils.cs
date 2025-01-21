using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using BackendService_IntegrationTests.Utils.Mocks;
using BackendService.Gateway.Endpoints.SignInUser;
using BackendService.Services.Users.Domain;

namespace BackendService_IntegrationTests.Utils;

public static class RequestUtils
{
    public static T ParseResponse<T>(HttpResponseMessage response)
    {
        var content = response.Content.ReadAsStringAsync().Result;
        var parsed = JsonSerializer.Deserialize<T>(content);
        if (parsed == null)
            throw new Exception($"Failed to parse response: {content}");
        return parsed;
    }

    public static async Task<SignInUserResponse> SignIn(HttpClient client, User user)
    {
       var request = new
       {
          username = user.Username,
          password = UserMock.Password 
       };
       
       var signInResponse = await client.PostAsJsonAsync(SignInUserController.Endpoint, request);
       Assert.That(signInResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
       return RequestUtils.ParseResponse<SignInUserResponse>(signInResponse);
    }
}