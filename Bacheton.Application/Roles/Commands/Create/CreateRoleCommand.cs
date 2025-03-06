using ErrorOr;
using MediatR;

namespace Bacheton.Application.Roles.Commands.Create;

public record CreateRoleCommand(string Name, List<Guid> Permissions, string? Description = null) : IRequest<ErrorOr<Created>>;