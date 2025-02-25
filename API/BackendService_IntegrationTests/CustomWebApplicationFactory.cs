using BackendService.Common.Repositories;
using BackendService.Services.Users.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
        Environment.SetEnvironmentVariable("Jwt:Issuer", "http://localhost:5000");
        Environment.SetEnvironmentVariable("Jwt:Audience", "http://localhost:5000");
        Environment.SetEnvironmentVariable("Jwt:Key", "abcdefghijklmnopqrstuvwxyz1234567890");
        Environment.SetEnvironmentVariable("Jwt:AccessTokenExpirationMinutes", "30");
        Environment.SetEnvironmentVariable("Jwt:RefreshTokenExpirationDays", "7");
        
        builder.UseEnvironment("Production");
        builder.UseTestServer();

        builder.ConfigureServices(services =>
        {
            services.Remove(services.Single(service => typeof(DbContextOptions<AppRepository>) == service.ServiceType));
            services.Remove(services.Single(service => typeof(AppRepository) == service.ServiceType));
            services.AddDbContext<AppRepository>((_, option) => option.UseNpgsql(GetConnectionString()));
        });
    }
    
    public void SeedUserData(Action<AppRepository> seedAction)
    {
        using var scope = Services.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<AppRepository>();
        seedAction(userRepository);
    }
    
    public async Task<User?> GetUserById(string id)
    {
        using var scope = Services.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<AppRepository>();
        return await userRepository.Users.FindAsync(id);
    }

    private void ClearUserData()
    {
        using var scope = Services.CreateScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<AppRepository>();
        var users = userRepository.Users.ToList();
        userRepository.Users.RemoveRange(users);
        userRepository.SaveChanges();
    }
    
    public void ClearData()
    {
        ClearUserData();
    }
}