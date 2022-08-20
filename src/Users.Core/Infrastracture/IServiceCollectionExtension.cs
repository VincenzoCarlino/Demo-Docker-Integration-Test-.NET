namespace Users.Core.Infrastracture;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

using System;

using Users.Core.Domain.Configurations;
using Users.Core.Domain.Repositories;
using Users.Core.Infrastracture.Persistence;
using Users.Core.Infrastracture.Persistence.Repositories;

internal static class IServiceCollectionExtension
{
    public static void AddInfrastracture(
        this IServiceCollection services,
        Func<IServiceProvider, IPersistenceConfiguration> persistenceConfigurationDelegate)
    {
        services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(
            (provider, options) =>
            {
                options.UseNpgsql(
                    persistenceConfigurationDelegate(provider).GetConnectionString()
                );
            }
        );

        using (var provider = services.BuildServiceProvider())
        {
            var persistenceConfiguration = persistenceConfigurationDelegate(provider);

            using (var dbContext = DesignTimeDbContextFactory.Create(persistenceConfiguration))
            {
                Console.WriteLine("Start applying migrations");
                dbContext.Database.Migrate();
                Console.WriteLine("End migrations");
            }
        }

        services.AddScoped<IUserRepository, UserRepository>();
    }
}
