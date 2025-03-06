using Bacheton.Application.Common.Results;
using Bacheton.Application.User.Common;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.User.Queries.List;

public record ListUsersQuery(int Page, int PageSize, string? Name) : IRequest<ErrorOr<ListResult<UserResult>>>;