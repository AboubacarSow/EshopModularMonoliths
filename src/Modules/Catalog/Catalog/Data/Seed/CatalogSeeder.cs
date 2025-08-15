using Shared.Data.Seed;

namespace Catalog.Data.Seed;

internal class CatalogSeeder(CatalogDbContext context) : ISeeder
{
    public async Task SeedAllAsync()
    {
        if (!context.Products.Any())
        {
            await context.AddRangeAsync(ProductConfig.Products);
            await context.SaveChangesAsync();
        }
    }
}