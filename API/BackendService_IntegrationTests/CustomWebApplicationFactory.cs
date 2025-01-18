using DocumentService.Users.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Testcontainers.PostgreSql;

namespace BackendService_IntegrationTests;

public class CustomWebApplicationFactory: WebApplicationFactory<Program>
{
    private readonly PostgreSqlContainer _dbContainer = new PostgreSqlBuilder().Build();
    
    private string GetConnectionString() => _dbContainer.GetConnectionString();
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        _dbContainer.StartAsync().GetAwaiter().GetResult();
        Environment.SetEnvironmentVariable("ConnectionStrings:DefaultConnection", GetConnectionString());
        
        builder.UseEnvironment("Production");
        builder.UseTestServer();

        builder.ConfigureServices(services =>
        {
            services.Remove(services.Single(service => typeof(DbContextOptions<UserRepository>) == service.ServiceType));
            services.Remove(services.Single(service => typeof(UserRepository) == service.ServiceType));
            services.AddDbContext<UserRepository>((_, option) => option.UseNpgsql(GetConnectionString()));
        });
    }
}