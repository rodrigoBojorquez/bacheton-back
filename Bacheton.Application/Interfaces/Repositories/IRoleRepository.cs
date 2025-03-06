using Bacheton.Domain.Entities;
using ErrorOr;

namespace Bacheton.Application.Interfaces.Repositories;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByUserIdAsync(Guid userId);
    Task<Role?> GetRoleByNameAsync(string roleName);
    Task<ErrorOr<Created>> AssignPermissionsAsync(Guid roleId, List<Guid> permissionIds);
}