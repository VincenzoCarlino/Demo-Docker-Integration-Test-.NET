namespace Users.Core.Infrastracture.Persistence;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using System;

using Users.Core.Domain.Configurations;

internal class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var connectionString = args[0];
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            throw new Exception("Missing connection string");
        }
        var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
        builder.UseNpgsql(
            connectionString
        );
        return new ApplicationDbContext(builder.Options);
    }

    internal static ApplicationDbContext Create(IPersistenceConfiguration persistenceConfiguration)
        => new DesignTimeDbContextFactory().CreateDbContext(new[] { persistenceConfiguration.GetConnectionString() });
}