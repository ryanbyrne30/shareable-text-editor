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

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var appConfig = new AppConfig{ WebSocketApiUrl = ""};
builder.Configuration.GetSection("AppConfig").Bind(appConfig);
builder.Services.AddSingleton(appConfig);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<Repository>(options => options.UseSqlite(connectionString));
builder.Services.AddSingleton<RepositoryFactory>();

builder.Services.AddTransient<CreateDocumentActionService>();
builder.Services.AddTransient<CreateSessionService>();
builder.Services.AddTransient<CreateDocumentService>();
builder.Services.AddTransient<DeleteSessionService>();
builder.Services.AddTransient<NewSessionMessageService>();
builder.Services.AddTransient<WebSocketApiService>();
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
