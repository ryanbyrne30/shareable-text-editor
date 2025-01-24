using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils;
using BackendService_IntegrationTests.Utils.Mocks;
using BackendService.Gateway.Endpoints.GetCurrentUser;
using BackendService.Gateway.Endpoints.SignInUser;

namespace BackendService_IntegrationTests.Gateway;

public class GetAuthenticatedUserTest
{
    private readonly CustomWebApplicationFactory _factory = new();
    private const string Endpoint = "/api/v1/users/me";
    private const string SignInEndpoint = "/api/v1/auth/sign-in";
    
    [TearDown]
    public void TearDown()
    {
      _factory.ClearData();
    }
    
    [Test]
    public async Task validRequest_shouldReturnOk()
    {
       var user = UserMock.GenerateUser();
       _factory.SeedUserData(context =>
       {
          context.Users.Add(user);
          context.SaveChanges();
       });
       var client = _factory.CreateClient();
       var request = new
       {
          username = user.Username,
          password = UserMock.Password 
       };
       
       var signInResponse = await client.PostAsJsonAsync(SignInEndpoint, request);
       Assert.That(signInResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
       
       var signInBody = RequestUtils.ParseResponse<SignInUserResponse>(signInResponse);
       
       client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signInBody.AccessToken);
       
       var response = await client.GetAsync(Endpoint);
       Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
       
       var body = RequestUtils.ParseResponse<GetCurrentUserResponse>(response);
       Assert.That(body.Id, Is.Not.Null);
       Assert.That(body.Username, Is.Not.Null);
       TestUtils.AssertDatesAreEqual(user.CreatedAt, body.CreatedAt);
       TestUtils.AssertDatesAreEqual(user.UpdatedAt, body.UpdatedAt);
    }
    
    [Test]
      public async Task unauthenticatedRequest_shouldReturnUnauthorized()
      {
         var client = _factory.CreateClient();
         var response = await client.GetAsync(Endpoint);
         Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
      }
}