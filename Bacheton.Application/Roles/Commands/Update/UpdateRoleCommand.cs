using ErrorOr;
using MediatR;

namespace Bacheton.Application.Roles.Commands.Update;

public record UpdateRoleCommand(Guid Id, string Name, List<Guid> Permissions, string? Description = null) : IRequest<ErrorOr<Updated>>;