using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Data;
using Shared.Data.Seed;
using Catalog.Data.Seed;
using Shared.Data.Interceptors;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Reflection;
using Shared.Behaviors;
namespace Catalog;

public static class CatalogModule
{
    public static IServiceCollection AddCatalogModule(this IServiceCollection services,IConfiguration configuration)
    {
        //Registering mediatR to the container
        services.AddMediatR(configuration =>
        {
            configuration.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

            //Configuration of pipeline behaviors
            //1. configuring ValidationBehavior
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //2. configuring loggingBehaviors
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        //Registering fluentValidation to the container
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        // Adding Interceptors inside of DI
        services.AddScoped<ISaveChangesInterceptor,AuditableEntityInterceptor>();
        services.AddScoped<ISaveChangesInterceptor,DispatchDomainEventsInterceptor>();

        services.AddDbContext<CatalogDbContext>((serviceProvider,options)=>
        {
            // Registering Interceptors
            options.AddInterceptors(serviceProvider.GetServices<ISaveChangesInterceptor>());  
            options.UseNpgsql(configuration.GetConnectionString("Database"));
        });
        services.AddTransient<ISeeder, CatalogSeeder>();
        return services;
    }
    public static IApplicationBuilder UseCatalogModule(this IApplicationBuilder app)
    {
        //This performs an auto-migration when application starts running;

        //We have an async method that get called inside of an non-async method: that's reason why we got here GetAwaiter() and GetResult() methods
        // InitialiseDatabaseAsync(app).GetAwaiter().GetResult();
        app.UseMigration<CatalogDbContext>();
        return app;
    }

    
}
