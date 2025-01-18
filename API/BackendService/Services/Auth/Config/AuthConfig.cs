using System.Text;
using BackendService.Common;
using BackendService.Services.Auth.UseCases;
using BackendService.Services.Auth.Utils;
using Microsoft.IdentityModel.Tokens;

namespace BackendService.Services.Auth.Config;

public class AuthConfig
{
    private const int minKeyLength = 32;
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Key { get; set; }
    public int AccessTokenExpirationMinutes { get; set; } = 30;
    public int RefreshTokenExpirationDays { get; set; } = 7;
    
    public static void Setup(IServiceCollection services, IConfiguration configuration)
    {
        var issuer = ConfigUtil.GetEnvVar(configuration, "Jwt:Issuer");
        var audience = ConfigUtil.GetEnvVar(configuration, "Jwt:Audience");
        var issuerSigningKey = ConfigUtil.GetEnvVar(configuration, "Jwt:Key");
        var accessTokenExpirationMinutes = int.Parse(ConfigUtil.GetEnvVar(configuration, "Jwt:AccessTokenExpirationMinutes"));
        var refreshTokenExpirationDays = int.Parse(ConfigUtil.GetEnvVar(configuration, "Jwt:RefreshTokenExpirationDays"));
        
        if (issuerSigningKey.Length < minKeyLength) throw new Exception($"Issuer signing key must be at least {minKeyLength} characters long");
        
        services.Configure<AuthConfig>(options =>
        {
            options.Issuer = issuer;
            options.Audience = audience;
            options.Key = issuerSigningKey;
            options.AccessTokenExpirationMinutes = accessTokenExpirationMinutes;
            options.RefreshTokenExpirationDays = refreshTokenExpirationDays;
        });
        
        services.AddAuthentication("Bearer").AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey))
            };
        });
        services.AddAuthorization();
        services.AddTransient<SignInUserService>();
        services.AddTransient<TokenUtil>();
    }
}