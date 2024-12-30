using DocumentAPI.Processes.CreateDocument;
using DocumentAPI.Processes.CreateSession;
using DocumentAPI.Processes.DeleteSession;
using DocumentAPI.Processes.NewSessionMessage;
using DocumentAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<Repository>(options => options.UseSqlite(connectionString));
builder.Services.AddTransient<CreateDocumentActionService>();
builder.Services.AddTransient<CreateSessionService>();
builder.Services.AddTransient<CreateDocumentService>();
builder.Services.AddTransient<DeleteSessionService>();
builder.Services.AddTransient<NewSessionMessageService>();

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
