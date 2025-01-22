using System.Net;
using System.Net.Http.Json;
using BackendService_IntegrationTests.Utils;
using BackendService.Gateway.Endpoints.CreateDocument;

namespace BackendService_IntegrationTests.Gateway;

public class CreateDocumentTest
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
        var client = _factory.CreateClient();
        var user = UserUtils.CreateUser(_factory); 
        await UserUtils.SignInUser(client, user);
        
        var response = await client.PostAsJsonAsync("/api/v1/documents", new {});
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        var body = RequestUtils.ParseResponse<CreateDocumentResponse>(response);
        Assert.That(body.Id, Is.Not.Null);
    }
    
    [Test]
    public async Task unauthorizedRequest_shouldReturnUnauthorized()
    {
        var client = _factory.CreateClient();
        
        var response = await client.PostAsJsonAsync("/api/v1/documents", new {});
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.Unauthorized));
    }
}