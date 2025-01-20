using System.Net;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils;
using BackendService_IntegrationTests.Utils.Mocks;
using BackendService.Gateway.Endpoints.RegisterUser;

namespace BackendService_IntegrationTests.Gateway;

public class CreateUserTests 
{
    private readonly CustomWebApplicationFactory _factory = new();
    
    [TearDown]
    public void TearDown()
    {
        _factory.ClearData();
    }

    [Test]
    public async Task validRequest_shouldCreateUser()
    {
        const string username = "username";
        const string password = "Pa$$word1";
        var request = new RegisterUserRequest(username, password);
        var client = _factory.CreateClient();
        
        var createResponse = await client.PostAsJsonAsync("/api/v1/users", request);
        
        Assert.That(createResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        
        var body = RequestUtils.ParseResponse<RegisterUserResponse>(createResponse);
        Assert.That(body.Id, Is.Not.Null);

        // confirm user is persisted
        var user = await _factory.GetUserById(body.Id);
        Assert.Multiple(() =>
        {
            Assert.That(user, Is.Not.Null);
            Assert.That(user?.Username, Is.EqualTo(username));
            Assert.That(user?.UpdatedAt, Is.Null);
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
        var request = new RegisterUserRequest(username, password);
        var client = _factory.CreateClient();
        
        var response = await client.PostAsJsonAsync("/api/v1/users", request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.BadRequest));
    }
    
    [Test]
    public async Task duplicateUser_shouldReturnConflict()
    {
        var user = UserMock.GenerateUser();
        _factory.SeedUserData(context =>
        {
            context.Users.Add(user);
            context.SaveChanges();
        });
        var request = new RegisterUserRequest(user.Username, "Pa$$word1");
        var client = _factory.CreateClient();
        
        var response = await client.PostAsJsonAsync("/api/v1/users", request);
        
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Conflict));
    }
}