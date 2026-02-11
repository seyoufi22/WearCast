using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Persistence;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
    {
        
    }
    
    // DB Sets will go here

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}