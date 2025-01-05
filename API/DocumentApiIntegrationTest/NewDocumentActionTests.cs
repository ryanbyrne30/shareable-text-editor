using System.Net;
using System.Text;
using System.Text.Json;
using DocumentAPI.Domain;
using DocumentAPI.Endpoints.NewSessionMessage;
using DocumentAPI.Repositories;
using DocumentAPIIntegrationTests.Factories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace DocumentAPIIntegrationTests;

[TestFixture]
public class NewDocumentActionTests
{
    private WireMockServer _mockWebSocketServer;
    private DocumentApiWebApplicationFactory<Program> _factory;
    private HttpClient _client;
    private Repository _repository;

    [SetUp]
    public void Setup()
    {
        _mockWebSocketServer = WireMockServer.Start();
        _mockWebSocketServer.Given(Request.Create().WithPath("/sockets/socket/{socketId}").UsingPost())
            .RespondWith(Response.Create().WithBody("{\"message\":\"ok\"}").WithStatusCode(200));
        
        _factory = new DocumentApiWebApplicationFactory<Program>(_mockWebSocketServer.Urls.First());
        _client = _factory.CreateClient();

        var scopeFactory = _factory.Services.GetService<IServiceScopeFactory>();
        if (scopeFactory == null) throw new Exception("Could not get scopeFactory");
        var scope = scopeFactory.CreateScope();
        _repository = scope.ServiceProvider.GetRequiredService<Repository>();
    }

    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        _mockWebSocketServer.Dispose();
        _repository.Dispose();
    }

    [Test]
    public async Task ValidAction_PersistsAction()
    {
        // Arrange
        var action = NewSessionMessage.CreateActionMessage(1, 0, "a", 0);
        var request = new NewSessionMessageRequest { Message = JsonSerializer.Serialize(action) };
        var document = new Document
        {
            Id = Repository.GenerateId(Document.IdPrefix),
            Content = "",
            Title = "My Document",
            CreatedAt = DateTime.Now
        };

        var session1 = new Session
        {
            Id = Repository.GenerateId(Session.IdPrefix),
            DocumentId = document.Id,
            SocketId = "sock_111",
            CreatedAt = DateTime.Now
        };

        var session2 = new Session
        {
            Id = Repository.GenerateId(Session.IdPrefix),
            DocumentId = document.Id,
            SocketId = "sock_222",
            CreatedAt = DateTime.Now
        };

        _repository.Documents.Add(document);
        _repository.Sessions.AddRange([session1, session2]);
        await _repository.SaveChangesAsync();

        // Act
        var body = JsonSerializer.Serialize(request);
        var requestBody = new StringContent(body, Encoding.UTF8, "application/json");
        var response = await _client.PostAsync($"/sessions/session/{session1.Id}", requestBody); 
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        await Task.Delay(500);
        
        var wsRequest = _mockWebSocketServer.LogEntries.FirstOrDefault(l => l.RequestMessage.Path.Equals($"/sockets/socket/{session1.SocketId}"));
        Assert.That(wsRequest, Is.Not.Null);
        Assert.That(wsRequest.RequestMessage.Body, Is.EqualTo("{\"message\":\"ACK: 1\"}"));
    } 
}