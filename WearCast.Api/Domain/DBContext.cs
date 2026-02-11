using Microsoft.EntityFrameworkCore;

namespace WearCast.Api.Domain;

public class DBContext : DbContext
{
    public DBContext(DbContextOptions<DBContext> options):base(options)
    {
        
    }
    
    // DB Sets will go here

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        
    }
}