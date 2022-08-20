namespace Users.Core;

using Microsoft.Extensions.DependencyInjection;

using System;

using Users.Core.Domain.Configurations;
using Users.Core.Infrastracture;

public static class IServiceCollectionExtension
{
    public static void AddCore(
        this IServiceCollection services,
        Func<IServiceProvider, IPersistenceConfiguration> persistenceConfigurationDelegate)
    {
        services.AddInfrastracture(persistenceConfigurationDelegate);
    }
}