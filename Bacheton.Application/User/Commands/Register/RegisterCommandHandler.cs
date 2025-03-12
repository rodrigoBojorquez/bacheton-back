using Bacheton.Application.Interfaces.Repositories;
using Bacheton.Application.Interfaces.Services;
using Bacheton.Application.User.Common;
using Bacheton.Domain.Constants;
using Bacheton.Domain.Errors;
using ErrorOr;
using MediatR;

namespace Bacheton.Application.User.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordService _passwordService;

    public RegisterCommandHandler(IUserRepository userRepository, ITokenService tokenService, IRoleRepository roleRepository, IPasswordService passwordService)
    {
        _userRepository = userRepository;
        _tokenService = tokenService;
        _roleRepository = roleRepository;
        _passwordService = passwordService;
    }

    public async Task<ErrorOr<AuthResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user is not null)
            return Errors.User.EmailAlreadyExists;

        var userRole = await _roleRepository.GetRoleByNameAsync(BachetonConstants.Roles.UserRole);
        
        if (userRole is null)
            return Errors.Role.NotFound;

        var newUser = new Domain.Entities.User
        {
            Name = request.Name,
            Email = request.Email,
            Password = _passwordService.HashPassword(request.Password),
            RoleId = userRole.Id
        };
        await _userRepository.InsertAsync(newUser);

        var token = await _tokenService.GenerateTokenAsync(newUser);
        var refreshToken = _tokenService.GenerateRefreshToken();

        await _tokenService.StoreRefreshTokenAsync(refreshToken, newUser.Id);

        return new AuthResult(token, refreshToken);
    }
}