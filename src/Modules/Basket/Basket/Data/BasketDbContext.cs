using System.Reflection;
using Basket.Basket.Models;
using Microsoft.EntityFrameworkCore;

namespace Basket.Data;

internal class BasketDbContext : DbContext
{
    internal DbSet<ShoppingCartItem> ShoppingCartItems => Set<ShoppingCartItem>();
    internal DbSet<ShoppingCart> ShoppingCarts => Set<ShoppingCart>();

    public BasketDbContext(DbContextOptions<BasketDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("basket");
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }
}