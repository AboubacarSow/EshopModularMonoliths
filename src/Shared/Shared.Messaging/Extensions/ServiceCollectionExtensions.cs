using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransitWitAssemblies(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            config.SetInMemorySagaRepositoryProvider();

            config.AddConsumers(assemblies);

            config.AddSagaStateMachines(assemblies);
            config.AddSagas(assemblies);
            config.AddActivities(assemblies);

            config.UsingInMemory((context, configurator)=>
            {
                configurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}