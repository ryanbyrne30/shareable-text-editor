using DocumentAPI.Repositories;
using DocumentAPIIntegrationTests.Factories;
using Microsoft.Extensions.DependencyInjection;
using WireMock.Server;

namespace DocumentAPIIntegrationTests.Utils;

public class TestDependencies: IDisposable
{
    public WireMockServer MockWebSocketServer { get; }
    public HttpClient Client { get; }
    public Repository Repository { get; }
    
    public TestDependencies()
    {
        MockWebSocketServer = WireMockServer.Start();
        var webSocketServerUrl = MockWebSocketServer.Urls.First();
        
        var factory = new DocumentApiWebApplicationFactory<Program>(webSocketServerUrl); 
        Client = factory.CreateClient();
        
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>();
        if (scopeFactory == null) throw new Exception("Could not get scopeFactory");
        var scope = scopeFactory.CreateScope();
        Repository = scope.ServiceProvider.GetRequiredService<Repository>();
    }
    
    public void Dispose()
    {
        MockWebSocketServer.Stop();
        Client.Dispose();
        Repository.Dispose();
        GC.SuppressFinalize(this);
    }
}