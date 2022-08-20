namespace Users.Tests;

using DockerDotNet.Presets;
using DockerDotNet.Presets.Configurations.Postgres;
using DockerDotNet.Presets.DTO;

using Microsoft.Extensions.DependencyInjection;

using NUnit.Framework;

using System.Collections.Generic;
using System.Threading.Tasks;

using Users.Core;
using Users.Tests.Configuration;

[SetUpFixture]
internal class SetUp
{
    private readonly List<DockerContainerResult> _containers = new();
    private static IServiceScopeFactory _serviceScopeFactory;

    [OneTimeSetUp]
    public async Task RunBeforeAnyTestAsync()
    {
        var postgresConfiguration = PostgresConfiguration.Create();

        var pgsqlDockerContainer = await DockerContainerGenerator.CreateContainerAsync(
            new PostgresContainerConfiguration(
                postgresConfiguration.DbName,
                postgresConfiguration.User,
                postgresConfiguration.Password,
                PostgresConfiguration.CONTAINER_NAME,
                PostgresConfiguration.VOLUME_NAME,
                PostgresConfiguration.IMAGE_TAG,
                postgresConfiguration.Port
            )
        ).ConfigureAwait(false);

        if (pgsqlDockerContainer.Port != postgresConfiguration.Port)
        {
            postgresConfiguration.UpdatePort(pgsqlDockerContainer.Port);
        }

        _containers.Add(pgsqlDockerContainer);

        await Task.Delay(5000).ConfigureAwait(false);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddCore(
            _ => postgresConfiguration
        );

        _serviceScopeFactory = serviceCollection.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
    }

    [OneTimeTearDown]
    public async Task RunAfterAllTestsAsync()
    {
        foreach (var container in _containers)
        {
            //await DockerContainerGenerator.DropContainerById(container.ContainerId).ConfigureAwait(false);
        }
    }

    public static IServiceScopeFactory GetServiceScopeFactory() => _serviceScopeFactory;
}
