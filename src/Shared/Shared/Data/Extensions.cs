using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data.Seed;
namespace Shared.Data;

public static class Extensions
{
    public static void UseMigration<TContext>(this IApplicationBuilder app)
    where TContext : DbContext
    {
        MigrateDatabaseAsync<TContext>(app.ApplicationServices).GetAwaiter().GetResult();
        SeedDataAsync(app.ApplicationServices).GetAwaiter().GetResult();
    }

    private static async Task SeedDataAsync(IServiceProvider applicationServices)
    {
        var scope = applicationServices.CreateScope();
        //Here we got an instance of CatalogSeeder
        var seeders = scope.ServiceProvider.GetServices<ISeeder>();
        foreach (var seeder in seeders)
            await seeder.SeedAllAsync();
    
    }

    private static async Task MigrateDatabaseAsync<TContext>(IServiceProvider applicationServices)
        where TContext : DbContext
    {
        var scope = applicationServices.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<TContext>();
        if (context.Database.GetPendingMigrations().Any())
            await context.Database.MigrateAsync();
        
    }
}