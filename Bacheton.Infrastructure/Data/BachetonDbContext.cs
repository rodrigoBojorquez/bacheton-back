using System.Reflection;
using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Module = Bacheton.Domain.Entities.Module;

namespace Bacheton.Infrastructure.Data;

public class BachetonDbContext : DbContext
{
    public BachetonDbContext(DbContextOptions<BachetonDbContext> options) : base(options)
    {
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Permission> Permissions { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Module> Modules { get; set; }
    public DbSet<PermissionRole> PermissionRoles { get; set; }
    public DbSet<Report> Reports { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}