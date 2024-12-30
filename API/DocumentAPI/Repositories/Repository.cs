using DocumentAPI.Domain;
using Microsoft.EntityFrameworkCore;

namespace DocumentAPI.Repositories;

public class Repository(DbContextOptions<Repository> options) : DbContext(options)
{
    public DbSet<Document> Documents { get; init; }
    public DbSet<DocumentAction> DocumentActions { get; init; }
    public DbSet<Session> Sessions { get; init; }
    
    public static string GenerateId(string prefix)
    {
        var uuid = Guid.NewGuid();
        return prefix + '_' + uuid.ToString().Replace("-", "");
    }
}