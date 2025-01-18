using BackendService.Services.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace BackendService.Services.Users.Repository;

public class UserRepository(DbContextOptions options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
}