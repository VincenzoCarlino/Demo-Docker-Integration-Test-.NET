namespace Users.Core.Infrastracture.Persistence.EntitiesConfiguration;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using Users.Core.Domain.Models;

internal class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(x => x.Id);

        builder.Property(e => e.Id)
            .HasColumnName("id");
        builder.Property(e => e.FirstName)
            .HasColumnName("first_name");
        builder.Property(e => e.LastName)
            .HasColumnName("last_name");
        builder.Property(e => e.Email)
            .HasColumnName("email");
    }
}
