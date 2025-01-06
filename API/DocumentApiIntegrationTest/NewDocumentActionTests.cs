using System.Net;
using DocumentAPI.Domain;
using DocumentAPI.Endpoints.NewSessionMessage;
using DocumentAPI.Processes.DocumentWatcher;
using DocumentAPI.Repositories;
using DocumentAPIIntegrationTests.Utils;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DocumentAPIIntegrationTests;

[TestFixture]
public class NewDocumentActionTests
{
    private TestDependencies _deps; 

    [SetUp]
    public void Setup()
    {
        _deps = new TestDependencies();
        _deps.MockWebSocketServer.Given(Request.Create().WithPath("/sockets/socket/{socketId}").UsingPost())
            .RespondWith(Response.Create().WithBody("{\"message\":\"ok\"}").WithStatusCode(200));
    }

    [TearDown]
    public void TearDown() => _deps.Dispose();

    [Test]
    public async Task ValidAction_SendsActionToClients()
    {
        // Arrange
        var requestBody = NewSessionMessageRequest.CreateActionMessage(1, 0, "a", 0);
        var expectedAck = SocketMessage.CreateAck(1);
        var expectedAction = SocketMessage.CreateAction(1, 0, "a", 0);
        
        var document = new Document(Repository.GenerateId(Document.IdPrefix));
        _deps.Repository.Documents.Add(document);

        var session1 = new Session(Repository.GenerateId(Session.IdPrefix), document.Id, "sock_1");
        var session2 = new Session(Repository.GenerateId(Session.IdPrefix), document.Id, "sock_2");
        _deps.Repository.Sessions.AddRange([session1, session2]);
        
        await _deps.Repository.SaveChangesAsync();

        // Act
        var response = await _deps.Client.PostAsync($"/sessions/session/{session1.Id}", RequestUtils.JsonContent(requestBody)); 
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));

        await Task.Delay(1000);
        
        // Assert
        var ackRequest = _deps.MockWebSocketServer.LogEntries.FirstOrDefault(l => l.RequestMessage.Path.Equals($"/sockets/socket/{session1.SocketId}"));
        var ackRequestBody = ackRequest?.RequestMessage.Body;
        Assert.That(ackRequestBody, Is.Not.Null);
        var ackMessage = JsonSerializer.Deserialize<SocketMessage>(ackRequestBody);
        Assert.That(ackMessage?.Ack?.Revision, Is.EqualTo(expectedAck.Ack?.Revision));
        
        var actionRequest = _deps.MockWebSocketServer.LogEntries.FirstOrDefault(l => l.RequestMessage.Path.Equals($"/sockets/socket/{session2.SocketId}"));
        var actionRequestBody = actionRequest?.RequestMessage.Body;
        Assert.That(actionRequestBody, Is.Not.Null);
        var actionMessage = JsonSerializer.Deserialize<SocketMessage>(actionRequestBody);
        Assert.Multiple(() =>
        {
            Assert.That(actionMessage?.Action?.Revision, Is.EqualTo(expectedAction.Action?.Revision));
            Assert.That(actionMessage?.Action?.Position, Is.EqualTo(expectedAction.Action?.Position));
            Assert.That(actionMessage?.Action?.Inserted, Is.EqualTo(expectedAction.Action?.Inserted));
            Assert.That(actionMessage?.Action?.Deleted, Is.EqualTo(expectedAction.Action?.Deleted));
        });
    } 
}