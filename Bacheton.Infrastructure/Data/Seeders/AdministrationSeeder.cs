using Bacheton.Domain.Entities;
using Bacheton.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bacheton.Infrastructure.Data.Seeders;

public static partial class Seeder
{
    public static class Administration
    {
        /**
         * Seeder para la gestion de roles de la aplicación
         */
        public static async Task SeedAsync(DbContext context)
        {
            if (await context.Set<Module>().AnyAsync() ||
                await context.Set<Permission>().AnyAsync() ||
                await context.Set<Role>().AnyAsync() ||
                await context.Set<User>().AnyAsync())
            {
                return;
            }

            // Insertar modulos
            var modules = new[]
            {
                new Module { Name = "Usuarios", Description = "Módulo de gestión de usuarios", Icon = "pi pi-users"},
                new Module { Name = "Roles", Description = "Módulo de gestión de roles", Icon = "pi pi-shield"},
                new Module { Name = "Permisos", Description = "Módulo de gestión de permisos", Icon = "pi pi-lock"},
                new Module { Name = "Reportes", Description = "Modulo de reportes de bacheo", Icon = "pi pi-flag"},    
            };

            await context.Set<Module>().AddRangeAsync(modules);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var modulesIds = modules.ToDictionary(m => m.Name, m => m.Id);


            // Insertar permisos
            var permissionNames = new[]
            {
                new { Name = "create", DisplayName = "Crear", Icon = "pi pi-plus" },
                new { Name = "read", DisplayName = "Ver", Icon = "pi pi-eye" },
                new { Name = "update", DisplayName = "Actualizar", Icon = "pi pi-pencil" },
                new { Name = "delete", DisplayName = "Eliminar", Icon = "pi pi-trash" }
            };

            var permissions = modules
                .Where(m => m.Name != "Permisos")
                .SelectMany(m => permissionNames.Select(p => new Permission { Name = p.Name, DisplayName = p.DisplayName, Icon = p.Icon, ModuleId = m.Id }))
                .ToList();

            permissions.AddRange(new[]
            {
                new Permission { Name = "superAdmin", IsPublic = false, DisplayName = "Super acceso", Icon = "pi pi-star" },
                new Permission { Name = "resolve", ModuleId = modulesIds["Reportes"], DisplayName = "Resolver", Icon = "pi pi-check" },
                new Permission { Name = "monitoring", ModuleId = modulesIds["Reportes"], DisplayName = "Monitorear", Icon = "pi pi-chart-line" },
                new Permission { Name = "read", ModuleId = modulesIds["Permisos"], DisplayName = "Ver", Icon = "pi pi-eye" },
                new Permission { Name = "update", ModuleId = modulesIds["Permisos"], DisplayName = "Actualizar", Icon = "pi pi-pencil" },
            });

            await context.Set<Permission>().AddRangeAsync(permissions);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();
            

            var permissionsIds = permissions
                .Where(p => p.ModuleId.HasValue)
                .ToDictionary(p => $"{p.Name}:{modulesIds.FirstOrDefault(m => m.Value == p.ModuleId).Key}", p => p.Id);


            // Insertar roles
            var roles = new[]
            {
                new Role { Name = "Administrador", Description = "Acceso total al sistema" },
                new Role { Name = "Usuario", Description = "Acceso a las funcionalidades no administrativas" },
                new Role { Name = "Supervisor", Description = "Acceso a las funcionalidades de supervisión" },
            };

            await context.Set<Role>().AddRangeAsync(roles);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();

            var rolesIds = roles.ToDictionary(r => r.Name, r => r.Id);


            // Insertar usuarios
            var users = new[]
            {
                new User
                {
                    Name = "Rodrigo", Email = "rbojorquez1620@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"), RoleId = rolesIds["Administrador"]
                },
                new User
                {
                    Name = "Andres", Email = "andres@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"), RoleId = rolesIds["Administrador"]
                },
                new User
                {
                    Name = "Dylan", Email = "dylan@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"), RoleId = rolesIds["Administrador"]
                },
                new User
                {
                    Name = "Ricardo", Email = "ricardo@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"), RoleId = rolesIds["Administrador"]
                },
                new User
                {
                    Name = "Juan Carlos", Email = "juan@gmail.com",
                    Password = BCrypt.Net.BCrypt.HashPassword("password"), RoleId = rolesIds["Administrador"]
                }
            };

            await context.Set<User>().AddRangeAsync(users);
            await context.SaveChangesAsync();
            context.ChangeTracker.Clear();


            // Insertar roles-permisos
            var rolesPermissions = roles
                .SelectMany(r => permissions.Select(p => new PermissionRole { RoleId = r.Id, PermissionId = p.Id }))
                .ToList();

            foreach (var rp in rolesPermissions)
            {
                Console.WriteLine($"Role: {rp.RoleId} - Permission: {rp.PermissionId}");
            }

            await context.Set<PermissionRole>().AddRangeAsync(rolesPermissions);
            await context.SaveChangesAsync();
        }
    }
}