using System.Linq.Expressions;
using Bacheton.Application.Common.Results;
using Bacheton.Application.Permissions.Common;
using Bacheton.Domain.Entities;

namespace Bacheton.Application.Interfaces.Repositories;

public interface IPermissionRepository : IRepository<Permission>
{
    Task<List<Permission>> GetByRoleAsync(Guid roleId);
    new Task<ListResult<PermissionResult>> ListAsync(Expression<Func<Permission, bool>>? filter = null);
}