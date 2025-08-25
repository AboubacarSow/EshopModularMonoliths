using System.Reflection;

namespace Basket.Data;

internal class BasketDbContext : DbContext
{
    internal DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();
    internal DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();
    internal DbSet<OutBoxMessage> OutBoxMessages => Set<OutBoxMessage>();

    public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("basket");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}