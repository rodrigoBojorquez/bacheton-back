using ErrorOr;
using MediatR;

namespace Bacheton.Application.User.Commands.Edit;

public record EditUserCommand(Guid Id, string Name, string Email, string Password, Guid RoleId) : IRequest<ErrorOr<Updated>>;