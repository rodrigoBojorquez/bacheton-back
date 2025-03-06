using Bacheton.Application.User.Common;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.User.Queries.Login;

public record LoginQuery(string Email, string Password) : IRequest<ErrorOr<AuthResult>>;