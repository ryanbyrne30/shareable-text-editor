using DocumentAPI.Config;
using DocumentAPI.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentAPIIntegrationTests.Factories;

public class DocumentApiWebApplicationFactory<TEntryPoint>(string mockWebSocketApiUrl) : WebApplicationFactory<TEntryPoint> where TEntryPoint : class
{
    private SqliteConnection? _sqliteConnection;
    
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Production");
        builder.ConfigureServices((_, services) =>
        {
            // override AppConfig
            var appConfig = services.SingleOrDefault(d => d.ServiceType == typeof(AppConfig));
            if (appConfig != null) services.Remove(appConfig);

            var newAppConfig = new AppConfig
            {
                ConnectionString = "Data Source=:memory:",
                WebSocketApiUrl = mockWebSocketApiUrl
            };
            services.AddSingleton(newAppConfig);
            
            // create long living in-memory database
            _sqliteConnection = new SqliteConnection("Data Source=:memory:");
            _sqliteConnection.Open();
            
            // remove old DbContext
            var dbContext = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<Repository>));
            if (dbContext != null) services.Remove(dbContext);
            
            // add new DbContext
            services.AddDbContext<Repository>(options =>
                options.UseSqlite(_sqliteConnection));

            // ensure database is created
            var sp = services.BuildServiceProvider();
            using var scope = sp.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var db = scopedServices.GetRequiredService<Repository>();
            db.Database.Migrate();
            db.Database.EnsureCreated();
        });
    }
    
    protected override void Dispose(bool disposing)
    {
        if (disposing) _sqliteConnection?.Dispose();
        base.Dispose(disposing);
    }
}