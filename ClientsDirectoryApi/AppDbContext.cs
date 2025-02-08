using Microsoft.EntityFrameworkCore;

namespace ClientsDirectoryApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Client> Clients { get; set; }
}