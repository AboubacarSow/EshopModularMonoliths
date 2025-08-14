using System.Reflection;

namespace Catalog.Data;
internal class CatalogDbContext: DbContext
{
    internal DbSet<Product> Products => Set<Product>();
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options):base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("catalog");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}