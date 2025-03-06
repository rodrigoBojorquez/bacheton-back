using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Domain.Entities;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Roles.Commands.Create;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, ErrorOr<Created>>
{
    private readonly IRoleRepository _roleRepository;

    public CreateRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<ErrorOr<Created>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = new Role
        {
            Name = request.Name,
            Description = request.Description
        };

        await _roleRepository.InsertAsync(role);
        await _roleRepository.AssignPermissionsAsync(role.Id, request.Permissions);

        return Result.Created;
    }
}