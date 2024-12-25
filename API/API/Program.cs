using API.ChatRoom.Services;
using API.Document;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddTransient<WebSocketClientService>();

builder.Services.AddTransient<DocumentClientService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseWebSockets();
app.UseRouting();
app.MapControllers();

app.UseHttpsRedirection();
app.Run();
