using System.Runtime.CompilerServices;
using DocumentAPI.Config;
using DocumentAPI.Endpoints.CreateDocument;
using DocumentAPI.Endpoints.CreateSession;
using DocumentAPI.Endpoints.DeleteSession;
using DocumentAPI.Endpoints.NewSessionMessage;
using DocumentAPI.Repositories;
using DocumentAPI.Services.DocumentWatcherManager;
using DocumentAPI.Services.HttpRequestService;
using DocumentAPI.Services.WebSocketAPIService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

[assembly: InternalsVisibleTo("DocumentApiIntegrationTest")]

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();

// AppConfig
builder.Services.Configure<AppConfig>(builder.Configuration.GetSection("AppConfig"));
builder.Services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppConfig>>().Value);

// Swagger
builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();

// Controllers
builder.Services.AddControllers();

// Database
builder.Services.AddDbContext<Repository>((serviceProvider, options) =>
{
    var config = serviceProvider.GetRequiredService<AppConfig>();
    options.UseSqlite(config.ConnectionString);
});
builder.Services.AddSingleton<RepositoryFactory>();

// Services
builder.Services.AddTransient<CreateDocumentActionService>();
builder.Services.AddTransient<CreateSessionService>();
builder.Services.AddTransient<CreateDocumentService>();
builder.Services.AddTransient<DeleteSessionService>();
builder.Services.AddTransient<NewSessionMessageService>();
builder.Services.AddTransient<IWebSocketApiService, WebSocketApiService>();
builder.Services.AddTransient<HttpRequestService>();
builder.Services.AddSingleton<DocumentWatcherManager>();


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<Repository>();
    dbContext.Database.Migrate();
    dbContext.Database.EnsureCreated();
}

app.MapSwagger();
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();
