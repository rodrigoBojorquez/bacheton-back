using Bacheton.Domain.Entities;

namespace Bacheton.Application.Interfaces.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<List<Permission>> GetByRoleAsync(Guid roleId);
}