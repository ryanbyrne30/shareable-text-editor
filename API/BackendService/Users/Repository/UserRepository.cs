using DocumentService.Users.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentService.Users.Repository;

public class UserRepository(DbContextOptions options): DbContext(options)
{
    public DbSet<User> Users { get; set; }
}