namespace Users.Tests;

using DockerDotNet.Presets;
using DockerDotNet.Presets.Configurations.Postgres;
using DockerDotNet.Presets.DTO;

using Microsoft.Extensions.DependencyInjection;

using Npgsql;

using NUnit.Framework;

using System;
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

        await WaitForPostgresToBeReasyAsync(postgresConfiguration)
            .ConfigureAwait(false);

        var serviceCollection = new ServiceCollection();

        serviceCollection.AddCore(
            _ => postgresConfiguration
        );

        _serviceScopeFactory = serviceCollection.BuildServiceProvider().GetRequiredService<IServiceScopeFactory>();
    }

    private async Task WaitForPostgresToBeReasyAsync(PostgresConfiguration postgresConfiguration)
    {
        var connectionString = string.Format(
            "Host={0};Username={1};Password={2};Database={3};Port={4}",
            "localhost",
            postgresConfiguration.User,
            postgresConfiguration.Password,
            postgresConfiguration.DbName,
            postgresConfiguration.Port
        );

        for (int i = 0; i < 10; i++)
        {
            try
            {
                using var connection = new NpgsqlConnection(connectionString);
                await connection.OpenAsync().ConfigureAwait(false);
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    return;
                }
            }
            catch (Exception _)
            {
                await Task.Delay(i * 1000).ConfigureAwait(false);
            }
        }

        throw new Exception("Unable to connect to postgres");
    }

    [OneTimeTearDown]
    public async Task RunAfterAllTestsAsync()
    {
        foreach (var container in _containers)
        {
            await DockerContainerGenerator.DropContainerById(container.ContainerId).ConfigureAwait(false);
        }
    }

    public static IServiceScopeFactory GetServiceScopeFactory() => _serviceScopeFactory;
}
