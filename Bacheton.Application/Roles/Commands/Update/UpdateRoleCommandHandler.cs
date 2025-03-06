using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Roles.Commands.Update;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ErrorOr<Updated>>
{
    private readonly IRoleRepository _roleRepository;

    public UpdateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<Updated>> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        
        if (role is null)
            return Errors.Role.NotFound;
        
        role.Name = request.Name;
        role.Description = request.Description;

        await _roleRepository.UpdateAsync(role);
        await _roleRepository.AssignPermissionsAsync(role.Id, request.Permissions);

        return Result.Updated;
    }
}