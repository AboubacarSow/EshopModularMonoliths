using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services,IConfiguration configuration)
    {
        var connectionString= configuration.GetConnectionString("Database");
        services.AddDbContext<CatalogDbContext>(options =>
        {
            options.UseNpgsql(connectionString);
        });
        return services;
    }
    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        //This performs an auto-migration when application starts running;

        //We have an async method that get called inside of an non-async method: that's reason why we got here GetAwaiter() and GetResult() methods
         InitialiseDatabaseAsync(app).GetAwaiter().GetResult();
        return app;
    }

    private static async Task InitialiseDatabaseAsync(IApplicationBuilder app)
    {
        var scope = app.ApplicationServices.CreateScope();

        var context= scope.ServiceProvider.GetRequiredService<CatalogDbContext>();

        if(context.Database.GetPendingMigrations().Any())
        {
            await context.Database.MigrateAsync();
        }
    }
}
