using Microsoft.EntityFrameworkCore;

namespace ClientsDirectoryApi;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Client> Clients { get; set; }
    public DbSet<Account> Accounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>()
            .HasOne(a => a.Client)
            .WithMany(c => c.Accounts)
            .HasForeignKey(a => a.ClientId)
            .OnDelete(DeleteBehavior.Cascade); 

        base.OnModelCreating(modelBuilder);
    }
}