using System.Diagnostics.Contracts;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using ProductCatalog.Core.Entities;

namespace ProductCatalog.DataAccess.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public DbSet<Product> Products { get; set; }
}