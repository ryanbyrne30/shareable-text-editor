using BackendService.Services.Users.Repository;
using BackendService.Services.Users.UseCases;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Services.Users.Config;

public static class UsersConfig
{
    public static void Setup(IServiceCollection services, string connectionString)
    {
        services.AddDbContext<UserRepository>(options => options.UseNpgsql(connectionString));
        services.AddTransient<CreateUserService>();
        services.AddTransient<GetUserByUserIdService>();
        services.AddTransient<GetUserByUsernameService>();
        services.AddTransient<VerifyUserPasswordService>();
    }
}