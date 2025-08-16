using System.Reflection;
using Carter;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Extensions;

public static class CarterInjectionExtension
{
    public static IServiceCollection  AddCarterWithAssemblies(this IServiceCollection services,
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
}