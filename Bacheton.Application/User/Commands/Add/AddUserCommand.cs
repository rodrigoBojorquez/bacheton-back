using ErrorOr;
using MediatR;

namespace Bacheton.Application.User.Commands.Add;

public record AddUserCommand(string Name, string Email, string Password, Guid RoleId) : IRequest<ErrorOr<Created>>;