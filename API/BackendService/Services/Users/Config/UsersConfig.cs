using BackendService.Services.Users.UseCases;

namespace BackendService.Services.Users.Config;

public static class UsersConfig
{
    public static void Setup(IServiceCollection services)
    {
        services.AddTransient<CreateUserService>();
        services.AddTransient<GetUserByUserIdService>();
        services.AddTransient<GetUserByUsernameService>();
        services.AddTransient<VerifyUserPasswordService>();
    }
}