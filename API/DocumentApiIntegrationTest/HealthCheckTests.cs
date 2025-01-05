using System.Net;
using DocumentAPIIntegrationTests.Factories;
using WireMock.Server;

namespace DocumentAPIIntegrationTests;

[TestFixture]
public class Tests
{
    private WireMockServer _mockWebSocketServer;
    private DocumentApiWebApplicationFactory<Program> _factory;
    private HttpClient _client;
    
    [SetUp]
    public void Setup()
    {
        _mockWebSocketServer = WireMockServer.Start();
        _factory = new DocumentApiWebApplicationFactory<Program>(_mockWebSocketServer.Urls.First());
        _client = _factory.CreateClient();
    }
    
    [TearDown]
    public void TearDown()
    {
        _client.Dispose();
        _factory.Dispose();
        _mockWebSocketServer.Dispose();
    }

    [Test]
    public async Task Test1()
    {
        // Arrange
        const HttpStatusCode expectedStatus = HttpStatusCode.OK;
        
        // Act
        var response = await _client.GetAsync("/health");
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(expectedStatus));
    }
}