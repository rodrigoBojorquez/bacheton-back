using Bacheton.Application.User.Common;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.User.Commands.Register;

public record RegisterCommand(string Name, string Email, string Password) : IRequest<ErrorOr<AuthResult>>;