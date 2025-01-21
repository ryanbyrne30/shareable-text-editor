using BackendService.Services.Auth.Domain;
using BackendService.Services.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Common.Repositories;

public class AppRepository(DbContextOptions options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    
}