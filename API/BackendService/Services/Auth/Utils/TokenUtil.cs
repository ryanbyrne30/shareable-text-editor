using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using BackendService.Services.Auth.Config;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BackendService.Services.Auth.Utils;

public class TokenUtil(IOptions<AuthConfig> authConfig, ILogger<TokenUtil> logger)
{
    public DateTime AccessTokenExpirationMinutes() => DateTime.Now.AddMinutes(authConfig.Value.AccessTokenExpirationMinutes).ToUniversalTime(); 
    public DateTime RefreshTokenExpirationMinutes() => DateTime.Now.AddDays(authConfig.Value.RefreshTokenExpirationDays).ToUniversalTime(); 
        
    public string CreateAccessToken(string userId, string username)
    {
        var config = authConfig.Value;
        
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, userId),
            new Claim(JwtRegisteredClaimNames.UniqueName, username),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: config.Issuer,
            audience: config.Audience,
            claims: claims,
            expires: AccessTokenExpirationMinutes(), 
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
    public string CreateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    
    public ClaimsPrincipal? ValidateAccessToken(string token)
    {
        var config = authConfig.Value;
        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Key));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = creds.Key,
            ValidateIssuer = true,
            ValidIssuer = config.Issuer,
            ValidateAudience = true,
            ValidAudience = config.Audience,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            return tokenHandler.ValidateToken(token, tokenValidationParameters, out _);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error validating access token: {Message}", ex.Message);
            return null;
        }
    }
}