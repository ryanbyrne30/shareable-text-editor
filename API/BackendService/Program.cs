using BackendService.Common.Middleware;
using BackendService.Services.Auth.UseCases;
using BackendService.Services.Users.Repository;
using BackendService.Services.Users.UseCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

// Users
builder.Services.AddDbContext<UserRepository>(options => options.UseNpgsql(connectionString));
builder.Services.AddTransient<CreateUserService>();
builder.Services.AddTransient<GetUserByUserIdService>();
builder.Services.AddTransient<VerifyUserPasswordService>();

// Auth
builder.Services.AddTransient<SignInUserService>();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var userRepository = scope.ServiceProvider.GetRequiredService<UserRepository>();
    userRepository.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();
app.UseRouting();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();

namespace BackendService
{
    public partial class Program { }
}