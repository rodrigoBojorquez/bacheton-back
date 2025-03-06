using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.User.Commands.Add;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, ErrorOr<Created>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordService _passwordService;

    public AddUserCommandHandler(IUserRepository userRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _passwordService = passwordService;
    }

    public async Task<ErrorOr<Created>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = new Domain.Entities.User()
        {
            Name = request.Name,
            Email = request.Email,
            RoleId = request.RoleId,
            Password = _passwordService.HashPassword(request.Password)
        };
        
        await _userRepository.InsertAsync(user);

        return Result.Created;
    }
}