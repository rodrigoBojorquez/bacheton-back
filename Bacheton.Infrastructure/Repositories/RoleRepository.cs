using System.Linq.Expressions;
using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Domain.Entities;
using Bacheton.Domain.Errors;
using Bacheton.Infrastructure.Data;
using ErrorOr;
using Microsoft.EntityFrameworkCore;

namespace Bacheton.Infrastructure.Repositories;

public class RoleRepository : GenericRepository<Role>, IRoleRepository
{
    public RoleRepository(BachetonDbContext context) : base(context)
    {
    }

    public async Task<Role?> GetByUserIdAsync(Guid userId)
    {
        return (await Context.Users
            .Where(u => u.Id == userId)
            .Select(u => u.Role)
            .FirstOrDefaultAsync());
    }

    public Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return Context.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName);
    }

    public async Task<ErrorOr<Created>> AssignPermissionsAsync(Guid roleId, List<Guid> permissionIds)
    {
        var role = await Context.Roles
            .Include(r => r.Permissions)
            .FirstOrDefaultAsync(r => r.Id == roleId);

        if (role is null)
            return Errors.Role.NotFound;

        var currentPermissions = role.Permissions.Select(p => p.Id).ToList();
        var permissionsToAdd = permissionIds.Except(currentPermissions).ToList();
        var permissionsToRemove = currentPermissions.Except(permissionIds).ToList();

        var permissionsToAddEntities = await Context.Permissions
            .Where(p => permissionsToAdd.Contains(p.Id))
            .ToListAsync();

        var permissionsToRemoveEntities = role.Permissions
            .Where(p => permissionsToRemove.Contains(p.Id))
            .ToList();

        foreach (var permission in permissionsToRemoveEntities)
        {
            role.Permissions.Remove(permission);
        }

        foreach (var permission in permissionsToAddEntities)
        {
            role.Permissions.Add(permission);
        }

        await Context.SaveChangesAsync();

        return Result.Created;
    }

    public new async Task<ErrorOr<ListResult<Role>>> ListAsync(int page = 1, int pageSize = 10, Expression<Func<Role, bool>>? filter = null)
    {
        var query = Context.Roles
            .Include(x => x.Permissions) // Asegurar que se carguen los permisos
            .AsQueryable();

        if (filter is not null)
            query = query.Where(filter);

        var totalItems = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new ListResult<Role>(page, pageSize, totalItems, items);
    }
}