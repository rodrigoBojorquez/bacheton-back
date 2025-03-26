using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Permissions.Common;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Permissions.Queries.Find;

public class FindPermissionQueryHandler : IRequestHandler<FindPermissionQuery, ErrorOr<PermissionResult>>
{
    private readonly IPermissionRepository _permissionRepository;

    public FindPermissionQueryHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<ErrorOr<PermissionResult>> Handle(FindPermissionQuery request,
        CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.Id);

        if (permission is null)
            return Errors.Permission.NotFound;

        return new PermissionResult(permission.Id, permission.Name, permission.DisplayName, permission.Icon,
            permission.ModuleId, permission.Module.Name, permission.ClientPath);
    }
}