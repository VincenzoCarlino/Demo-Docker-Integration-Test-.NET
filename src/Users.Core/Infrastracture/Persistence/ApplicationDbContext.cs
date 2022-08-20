namespace Users.Core.Infrastracture.Persistence;

using Microsoft.EntityFrameworkCore;

using Users.Core.Domain.Models;
using Users.Core.Infrastracture.Persistence.EntitiesConfiguration;

internal class ApplicationDbContext : DbContext
{
    internal DbSet<User> Users { get; private set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
