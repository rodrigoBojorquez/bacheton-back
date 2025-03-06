using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bacheton.Infrastructure.Data.Configurations;

public class PermissionConfig : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(p => p.Id);

        builder.HasMany<Role>()
            .WithMany(r => r.Permissions)
            .UsingEntity<PermissionRole>();
    }
}