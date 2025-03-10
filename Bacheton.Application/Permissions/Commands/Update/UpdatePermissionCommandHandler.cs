using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Permissions.Commands.Update;

public class UpdatePermissionCommandHandler : IRequestHandler<UpdatePermissionCommand, ErrorOr<Updated>>
{
    private readonly IPermissionRepository _permissionRepository;

    public UpdatePermissionCommandHandler(IPermissionRepository permissionRepository)
    {
        _permissionRepository = permissionRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdatePermissionCommand request, CancellationToken cancellationToken)
    {
        var permission = await _permissionRepository.GetByIdAsync(request.Id);

        if (permission is null) return Errors.Permission.NotFound;

        permission.DisplayName = request.DisplayName;
        permission.Icon = request.Icon;

        await _permissionRepository.UpdateAsync(permission);
        return Result.Updated;
    }
}