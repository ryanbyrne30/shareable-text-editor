using System.Net;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils;
using DocumentService.Users.Endpoints.CreateUser;
using DocumentService.Users.Endpoints.GetUserByUserId;

namespace BackendService_IntegrationTests.Users;

public class CreateUserTests 
{
    private readonly CustomWebApplicationFactory _webApplicationFactory = new();

    private HttpClient GetClient() => _webApplicationFactory.CreateClient();

    [OneTimeTearDown]
    public void Teardown()
    {
        _webApplicationFactory.Dispose();
    }
    
    [Test]
    public async Task validRequest_shouldCreateUser()
    {
        const string username = "username";
        const string password = "Pa$$word1";
        var request = new CreateUserRequest(username, password);
        var client = GetClient();
        
        var createResponse = await client.PostAsJsonAsync("/api/v1/users", request);
        
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var body = RequestUtils.ParseResponse<CreateUserResponse>(createResponse);
        Assert.That(body.Id, Is.Not.Null);

        // confirm user is persisted 
        var getResponse = await client.GetAsync($"/api/v1/users/{body.Id}");
        
        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var user = RequestUtils.ParseResponse<GetUserByUserIdResponse>(getResponse);
        Assert.Multiple(() =>
        {
            Assert.That(user, Is.Not.Null);
            Assert.That(user.Username, Is.EqualTo(username));
            Assert.That(user.UpdatedAt, Is.Null);
        });
    }
    
    [Test]
    [TestCase("", "")]
    [TestCase("a", "Pa$$word1")]
    [TestCase("abc@me", "Pa$$word1")]
    [TestCase("abcme!", "Pa$$word1")]
    [TestCase("abc.123", "password")]
    [TestCase("abc.123", "Password")]
    [TestCase("abc.123", "Pa1$")]
    [TestCase("abc.123", "Pa$$word")]
    public async Task badRequest_shouldReturnBadRequest(string username, string password)
    {
        var request = new CreateUserRequest(username, password);
        var client = GetClient();
        
        var response = await client.PostAsJsonAsync("/api/v1/users", request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
}