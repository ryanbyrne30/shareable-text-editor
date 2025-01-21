using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils;
using BackendService_IntegrationTests.Utils.Mocks;
using BackendService.Gateway.Endpoints.RefreshUserTokens;
using BackendService.Gateway.Endpoints.SignInUser;

namespace BackendService_IntegrationTests.Gateway;

public class RefreshUserTokensTest
{
    private readonly CustomWebApplicationFactory _factory = new();

    [TearDown]
    public void TearDown()
    {
        _factory.ClearData();
    }

    [Test]
    public async Task validRequest_shouldReturnOk()
    {
        // Arrange
        var client = _factory.CreateClient();
        var signInResponse = await SetupUser(client); 
       client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signInResponse.AccessToken);

       var request = new RefreshUserTokensRequest
       {
           RefreshToken = signInResponse.RefreshToken
       };
       
       // Act
       var response = await client.PostAsJsonAsync(RefreshUserTokensController.Endpoint, request);
       
       // Assert
       Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK)); 
       var body = RequestUtils.ParseResponse<RefreshUserTokensResponse>(response);
       Assert.Multiple(() =>
       {
           Assert.That(body.AccessToken, Is.Not.Null);
           Assert.That(body.RefreshToken, Is.Not.Null);
       });
    }
    
    [Test]
    public async Task invalidRequest_shouldReturnBadRequest()
    {
        // Arrange
        var client = _factory.CreateClient();
        var signInResponse = await SetupUser(client); 
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signInResponse.AccessToken);

        // Act
        var response = await client.PostAsJsonAsync(RefreshUserTokensController.Endpoint, new {});
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest)); 
    }
    
    [Test]
    public async Task badToken_shouldReturnUnauthorized()
    {
        // Arrange
        var client = _factory.CreateClient();
        var signInResponse = await SetupUser(client); 
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", signInResponse.AccessToken);

        var request = new RefreshUserTokensRequest
        {
            RefreshToken = "invalid"
        };
        
        // Act
        var response = await client.PostAsJsonAsync(RefreshUserTokensController.Endpoint, request);
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized)); 
    }
    
    private async Task<SignInUserResponse> SetupUser(HttpClient client)
    {
        var user = UserMock.GenerateUser();
        _factory.SeedUserData(context =>
        {
            context.Users.Add(user);
            context.SaveChanges();
        });
        return await RequestUtils.SignIn(client, user);
    }
}