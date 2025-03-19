using Bacheton.Application.User.Common;
using MediatR;

namespace Bacheton.Application.User.Queries.TopUsers;

public record TopUsersQuery() : IRequest<List<TopUserResult>>;