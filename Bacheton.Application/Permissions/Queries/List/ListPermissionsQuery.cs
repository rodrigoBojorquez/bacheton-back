using Bacheton.Application.Common.Results;
using Bacheton.Application.Permissions.Common;
using MediatR;

namespace Bacheton.Application.Permissions.Queries.List;

public record ListPermissionsQuery(string? DisplayName = null, Guid? RoleId = null)
    : IRequest<ListResult<PermissionResult>>;