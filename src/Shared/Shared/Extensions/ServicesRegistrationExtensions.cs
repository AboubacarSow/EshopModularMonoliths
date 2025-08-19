using System.Reflection;
using Carter;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Behaviors;

namespace Shared.Extensions;

public static class ServicesRegistrationExtensions
{
    public static IServiceCollection AddCarterWithAssemblies(this IServiceCollection services,
    params Assembly[] assemblies)
    {
        foreach (var assembly in assemblies)
        {
            var modules = assembly.GetTypes()
            .Where(t => t.IsAssignableTo(typeof(ICarterModule)))
            .ToArray();

            services.AddCarter(configurator: config =>
            {
                config.WithModules(modules);
            });
        }

        return services;
    }

    public static IServiceCollection AddMediatorFromAssemblies(this IServiceCollection services,
    params Assembly[] assemblies)
    {
        services.AddMediatR(configuration =>
        {
            foreach(var assembly in assemblies)
            {
                configuration.RegisterServicesFromAssembly(assembly);
            }
            //Configuration of pipeline behaviors
            //1. configuring ValidationBehavior
            configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
            //2. configuring loggingBehaviors
            configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
        });
        return services;
    }

    public static IServiceCollection AddValidatorsFromAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        foreach(var assembly in assemblies)
        {
            services.AddValidatorsFromAssembly(assembly);
        }
        return services;
    }
}