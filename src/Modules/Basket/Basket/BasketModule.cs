using System.Net.Security;
using Basket.Data;
using Basket.Data.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Basket;

public static class BasketModule
{
    public static IServiceCollection AddBasketModule(this IServiceCollection services,IConfiguration configuration)
    {
        services.AddScoped<IBasketRepository, BasketRepository>();
        //This approch of registring redis distributed cache is not efficient and less maintainable
        // And it is a manual registration
        //services.AddScoped<IBasketRepository>(provider =>
        //{
        //    var basketRepository = provider.GetRequiredService<BasketRepository>();
        //    return new CachedBasketRepository(basketRepository, provider.GetRequiredService<IDistributedCache>());
        //});

        services.Decorate<IBasketRepository, CachedBasketRepository>();

        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<BasketDbContext>((serviceProvider,options) =>
        {
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
        return services;
    }
    public static IApplicationBuilder UseBasketModule(this IApplicationBuilder app)
    {
        app.UseMigration<BasketDbContext>();
        return app;
    }
}
