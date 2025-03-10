using Bacheton.Application.Permissions.Common;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Permissions.Queries.Find;

public record FindPermissionQuery(Guid Id) : IRequest<ErrorOr<PermissionResult>>;