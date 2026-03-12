using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Domain.Users;

namespace Persistence.Configurations.Users
{
    public class RoleConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.ToTable("Roles");

            builder.Property(r => r.RoleName)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(r => r.Description)
                .HasMaxLength(200);

            builder.HasIndex(r => r.RoleName).IsUnique();
        }
    }
}