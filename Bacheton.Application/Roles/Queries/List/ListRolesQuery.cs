using Bacheton.Application.Common.Results;
using Bacheton.Application.Roles.Common;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.Roles.Queries.List;

public record ListRolesQuery(int Page, int PageSize, string? Name) : IRequest<ErrorOr<ListResult<RoleResult>>>;