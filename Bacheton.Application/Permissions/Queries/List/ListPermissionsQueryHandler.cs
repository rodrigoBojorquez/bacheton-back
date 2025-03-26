using System.Linq.Expressions;
using Bacheton.Application.Common.Results;
using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Permissions.Common;
using Bacheton.Domain.Entities;
using MediatR;

namespace Bacheton.Application.Permissions.Queries.List;

public class ListPermissionsQueryHandler : IRequestHandler<ListPermissionsQuery, ListResult<PermissionResult>>
{
    private readonly IPermissionRepository _permissionRepository;

    public ListPermissionsQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<ListResult<PermissionResult>> Handle(ListPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        Expression<Func<Permission, bool>>? filter = null;

        if (!string.IsNullOrEmpty(request.DisplayName))
            filter = p => p.DisplayName.ToLower().Contains(request.DisplayName.ToLower());

        if (request.RoleId.HasValue)
            filter = p => p.Roles.Any(r => r.Id == request.RoleId);
        
        var permissions = await _permissionRepository.ListAsync(filter);

        return permissions;
    }
}