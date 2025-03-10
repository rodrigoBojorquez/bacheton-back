using ErrorOr;
using MediatR;

namespace Bacheton.Application.Permissions.Commands.Update;

public record UpdatePermissionCommand(Guid Id, string DisplayName, string? Icon = null) : IRequest<ErrorOr<Updated>>;