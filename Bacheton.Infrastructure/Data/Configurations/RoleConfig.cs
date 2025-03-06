using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bacheton.Infrastructure.Data.Configurations;

public class RoleConfig : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(r => r.Id);

        builder.HasMany<Permission>()
            .WithMany(p => p.Roles)
            .UsingEntity<PermissionRole>();
    }
}