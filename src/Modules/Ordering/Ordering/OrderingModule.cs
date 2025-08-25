using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Ordering.Data;
using Shared.Data;
using Shared.Data.Interceptors;

namespace Ordering;

public static class OrderingModule
{
    public static IServiceCollection AddOrderingModule(this IServiceCollection services,
    IConfiguration configuration)
    {
        //Registering the interceptors
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
        services.AddDbContext<OrderingDbContext>((serviceProvider,options) =>
        {
            //Adding the interceptors to the dbcontext
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>() ?? Array.Empty<ISaveChangesInterceptor>());
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
        return services;
    }
    public static IApplicationBuilder UseOrderingModule(this IApplicationBuilder app)
    {
        app.UseMigration<OrderingDbContext>();
        return app;
    }
}
