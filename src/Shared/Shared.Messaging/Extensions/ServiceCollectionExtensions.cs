using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Shared.Messaging.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddMassTransitWitAssemblies(this IServiceCollection services,
    IConfiguration configuration, params Assembly[] assemblies)
    {
        services.AddMassTransit(config =>
        {
            config.SetKebabCaseEndpointNameFormatter();

            //This can change in prod- the storage is not persistante because it's done internally(inside the RAM)
            config.SetInMemorySagaRepositoryProvider();

            config.AddConsumers(assemblies);

            config.AddSagaStateMachines(assemblies);
            config.AddSagas(assemblies);
            config.AddActivities(assemblies);

            // A transport - Here it using InMemery Transport
            //this configuration changes once we are in prod
            //there we will be using RabbitMq - 
            //config.UsingInMemory((context, configurator)=>
            //{
            //    configurator.ConfigureEndpoints(context);
            //});

            config.UsingRabbitMq((context, busConfigurator) =>
            {
                busConfigurator.Host(new Uri(configuration["MessageBroker:Host"]!), host =>
                {
                    host.Username(configuration["MessageBroker:Username"]!);
                    host.Password(configuration["MessageBroker:Password"]!);
                });
                busConfigurator.ConfigureEndpoints(context);
            });
        });
        return services;
    }
}